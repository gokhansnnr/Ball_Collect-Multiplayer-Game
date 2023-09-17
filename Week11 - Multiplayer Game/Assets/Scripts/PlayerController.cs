using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector] //2- Id'yi g�r�nmez yapt�k. Unityde vs. oynama yap�lmas�n, de�er girilmesin diye.
    public int id; //1- Her kullan�c�n�n bir idsi vard�r.

    [Header("Info")]
    public float moveSpeed; //3-Hareket h�z�n� tutuyoruz.
    public float jumpForce; //4-Z�plama �zelli�i de verdik.

    [Header("Components")]
    public Rigidbody rig; //7- Oyuncunun rigidbodysi ile i�lemler yapaca��z.
    public Player photonPlayer; //8-
    public GameObject gameManager;

    private void Update()
    {//9-

        //Updatede oyuncunun olu�tu�u yeri sa�lama alal�m.
        if (photonView.IsMine)
        {//29-Kullan�c� bizsek hareket ve z�plama i�lemlerini yaps�n.
            Move(); //10- Durmadan bu fonksiyon �al��s�n.

            if (Input.GetKeyDown(KeyCode.Space)) //11- Bo�lu�a bas�ld�k�a atlas�n
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
    {//Atlama ile ilgili komutlar. Bast�k�a dikey y�nde hareket uygulayabiliriz.
        Ray ray = new Ray(transform.position, Vector3.down); //15- Yukar�dan a�a��ya do�ru ray uygulad�k.

        if (Physics.Raycast(ray, 0.7f)) //Rayde 0.7f gibi bir de�i�im varsa
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //16- Impulse tipinde (hafif sirkelenecek) forcemode uygulad�k. 

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
    {//Karakterin ortaya ��kmas�n� burada yapaca��z.
        photonPlayer = player; //17-Player� photonplayer a verdik.
        id = player.ActorNumber; //18-Actornumber ile oyuncu idsini ald�k.

        GameManager.instance.players[id - 1] = this; //19- Numaralar 1,2,3,4 diye gitti�inden -1 dedik.

        if (!photonView.IsMine)
        {//20-E�er ben de�ilsem.
            rig.isKinematic = true; //21-Ben de�ilsem di�erlerinin kinemati�ini true yap.
            //Fizik kurallar� burada kalkacak. Kendimiz true oldu�unda hareket sa�layaca��z.
        }
    }//35-Verilmedi


}
