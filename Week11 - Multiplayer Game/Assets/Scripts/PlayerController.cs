using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector] //2- Id'yi görünmez yaptýk. Unityde vs. oynama yapýlmasýn, deðer girilmesin diye.
    public int id; //1- Her kullanýcýnýn bir idsi vardýr.

    [Header("Info")]
    public float moveSpeed; //3-Hareket hýzýný tutuyoruz.
    public float jumpForce; //4-Zýplama özelliði de verdik.

    [Header("Components")]
    public Rigidbody rig; //7- Oyuncunun rigidbodysi ile iþlemler yapacaðýz.
    public Player photonPlayer; //8-
    public GameObject gameManager;

    private void Update()
    {//9-

        //Updatede oyuncunun oluþtuðu yeri saðlama alalým.
        if (photonView.IsMine)
        {//29-Kullanýcý bizsek hareket ve zýplama iþlemlerini yapsýn.
            Move(); //10- Durmadan bu fonksiyon çalýþsýn.

            if (Input.GetKeyDown(KeyCode.Space)) //11- Boþluða basýldýkça atlasýn
                TryJump();

        }

    }
    void Move()
    { //Harektle ilgili kodlar

        float x = Input.GetAxis("Horizontal") * moveSpeed; //12
        float z = Input.GetAxis("Vertical") * moveSpeed; //13

        rig.velocity = new Vector3(-x, rig.velocity.y, -z); //14- y yerine 0'da yazabiliriz.

    }

    void TryJump()
    {//Atlama ile ilgili komutlar. Bastýkça dikey yönde hareket uygulayabiliriz.
        Ray ray = new Ray(transform.position, Vector3.down); //15- Yukarýdan aþaðýya doðru ray uyguladýk.

        if (Physics.Raycast(ray, 0.7f)) //Rayde 0.7f gibi bir deðiþim varsa
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //16- Impulse tipinde (hafif sirkelenecek) forcemode uyguladýk. 

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            print(gameObject.tag);
            Destroy(other.gameObject);
            gameManager.GetComponent<GameManager>().spawnCount -= 1;

        }
    }

    [PunRPC] //Duyurduk
    public void Initialize(Player player)
    {//Karakterin ortaya çýkmasýný burada yapacaðýz.
        photonPlayer = player; //17-Playerý photonplayer a verdik.
        id = player.ActorNumber; //18-Actornumber ile oyuncu idsini aldýk.

        GameManager.instance.players[id - 1] = this; //19- Numaralar 1,2,3,4 diye gittiðinden -1 dedik.

        if (!photonView.IsMine)
        {//20-Eðer ben deðilsem.
            rig.isKinematic = true; //21-Ben deðilsem diðerlerinin kinematiðini true yap.
            //Fizik kurallarý burada kalkacak. Kendimiz true olduðunda hareket saðlayacaðýz.
        }
    }//35-Verilmedi


}
