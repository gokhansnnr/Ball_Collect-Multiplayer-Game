using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq; //GetPlayerdeki First komutu için.

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Spawn Barrier")]
    public float minXCord;
    public float maxXCord;
    public float minZCord;
    public float maxZCord;
    public float YCord;
    public float spawnCount; //Random.Range(35, 50);
    private float nowSpawnCount;
    public GameObject barrier;

    [Header("Stats")] //Ýstatistiklerle alakalý bilgiler tutacaðýz.
    public bool gameEnded = false; //1- Oyun mekanizmasýný için

    [Header("Player")]
    public string playerPrefabLocation; //5- Kullanýcýnýn prefabýnýn olduðu yer. Kaynaðýn adresi tutulacak.
    public Transform[] spawnPoints; //6- Oyuncularýn belirli yerlerde spawn olmasý için.
    public PlayerController[] players; //7- Kaç oyuncu varsa, burada tutulacak.
    private int playersInGame; //9- Oyunda kaç kiþi varsa

    public static GameManager instance; //10-Bu kontroller her bir oyuncuda olacak.

    private void Awake()
    {//Herbir oyuncu kendi instancesini kendisi tutacak.
        instance = this; //11-Bu þekilde gameControl ayarý yaptýk.
    }

    private void Start()
    {
        spawnCount = Random.Range(50, 75);
        nowSpawnCount = spawnCount;
        print(spawnCount);
        SpawnBarrier();

        players = new PlayerController[PhotonNetwork.PlayerList.Length]; //12- Oyuncu listesini oluþturduk.
        //PhotonNetworkteki playerlist uzunluðunu buraya aldýk.

        //16-Startla gelen herbir kiþi 
        photonView.RPC("ImInGame", RpcTarget.All); //RPC ile ImInGame fonk duyurulacak. RpcTarget.All olan kiþilere. Yani tüm kiþilere
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (spawnCount > 0 && nowSpawnCount != spawnCount)
            {
                SpawnBarrier();
                nowSpawnCount = spawnCount;
            }

        }
        
    }

    [PunRPC] //13- Her gelen kendisini duyuracak.
    void ImInGame()
    {
        playersInGame++; //14-Her gelen kiþi sayýyý arttýracak.

        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {//Gelen kiþi photonnetworkteki listenin uzunluðuysa
            SpawnPlayer(); //15-Oyuncu oluþturulacak
        }
    }

    void SpawnBarrier()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minXCord, maxXCord), YCord, Random.Range(minZCord, maxZCord));
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(barrier, spawnPosition, spawnRotation);
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation,
            spawnPoints[Random.Range(0, spawnPoints.Length)].position,
            Quaternion.identity); //17- Instantiate içerisine lokasyonu, spawnpointste de random olarak 3-4 yer belirledik.
                                  //Aralýk ise, kaç nokta belirlediysek o olacak. Onun da pozisyonunu aldýk. Dönme noktasýný da (quaternion) aldýk.
                                  //Kýsaca Obje, belirlenen noktada belirlenen þekliyle görünecek.

        PlayerController playerScript = playerObj.GetComponent<PlayerController>(); //18-Burada spawn oluþmasý saðlandý.
        //playerObj'deki playercontrollera eriþildi.

        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        //18-Oluþturmuþ olduðumuz initialize metodunu burada çaðýrdýk ve RPC olarak herkese duyurduk.
        //LocalPlayerde de duyurumuzu yaptýk.
    }

    //Playerlara buradan id ile eriþelim. Þapkanýn oyunculara verilmesi için yazýyoruz.
    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId); //19-Playerlardan id sini x'e eþitlediðimizin ilkini getir dedik.
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.First(x => x.gameObject == playerObj); //20
    }
}
