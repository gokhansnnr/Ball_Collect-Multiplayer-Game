using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq; //GetPlayerdeki First komutu i�in.

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

    [Header("Stats")] //�statistiklerle alakal� bilgiler tutaca��z.
    public bool gameEnded = false; //1- Oyun mekanizmas�n� i�in

    [Header("Player")]
    public string playerPrefabLocation; //5- Kullan�c�n�n prefab�n�n oldu�u yer. Kayna��n adresi tutulacak.
    public Transform[] spawnPoints; //6- Oyuncular�n belirli yerlerde spawn olmas� i�in.
    public PlayerController[] players; //7- Ka� oyuncu varsa, burada tutulacak.
    private int playersInGame; //9- Oyunda ka� ki�i varsa

    public static GameManager instance; //10-Bu kontroller her bir oyuncuda olacak.

    private void Awake()
    {//Herbir oyuncu kendi instancesini kendisi tutacak.
        instance = this; //11-Bu �ekilde gameControl ayar� yapt�k.
    }

    private void Start()
    {
        spawnCount = Random.Range(50, 75);
        nowSpawnCount = spawnCount;
        print(spawnCount);
        SpawnBarrier();

        players = new PlayerController[PhotonNetwork.PlayerList.Length]; //12- Oyuncu listesini olu�turduk.
        //PhotonNetworkteki playerlist uzunlu�unu buraya ald�k.

        //16-Startla gelen herbir ki�i 
        photonView.RPC("ImInGame", RpcTarget.All); //RPC ile ImInGame fonk duyurulacak. RpcTarget.All olan ki�ilere. Yani t�m ki�ilere
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
        playersInGame++; //14-Her gelen ki�i say�y� artt�racak.

        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {//Gelen ki�i photonnetworkteki listenin uzunlu�uysa
            SpawnPlayer(); //15-Oyuncu olu�turulacak
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
            Quaternion.identity); //17- Instantiate i�erisine lokasyonu, spawnpointste de random olarak 3-4 yer belirledik.
                                  //Aral�k ise, ka� nokta belirlediysek o olacak. Onun da pozisyonunu ald�k. D�nme noktas�n� da (quaternion) ald�k.
                                  //K�saca Obje, belirlenen noktada belirlenen �ekliyle g�r�necek.

        PlayerController playerScript = playerObj.GetComponent<PlayerController>(); //18-Burada spawn olu�mas� sa�land�.
        //playerObj'deki playercontrollera eri�ildi.

        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        //18-Olu�turmu� oldu�umuz initialize metodunu burada �a��rd�k ve RPC olarak herkese duyurduk.
        //LocalPlayerde de duyurumuzu yapt�k.
    }

    //Playerlara buradan id ile eri�elim. �apkan�n oyunculara verilmesi i�in yaz�yoruz.
    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId); //19-Playerlardan id sini x'e e�itledi�imizin ilkini getir dedik.
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.First(x => x.gameObject == playerObj); //20
    }
}
