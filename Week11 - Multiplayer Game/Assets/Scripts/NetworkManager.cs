using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //1- Photon dahil ettik.

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;//2- Herbir oyuncunun kat�l�m� i�in bir durum a�mam�z laz�m.
    //Oyunun a��lmas� �zel bir i�lemdir. O sebeple static dedik.
    private void Awake()
    {//Lobi a�ma, room a�ma gibi i�lemleri burada yap�yoruz.
        //Awake, Starttan bir t�k �nce ger�ekle�ir. Oyun olu�madan �nce buras� �al���r.

        if (instance != null && instance != this) //3-Bir oturum a��lm��sa (bo� de�ilse) ve instance �uanki oturum de�ilse (this),
            gameObject.SetActive(false); //4- Sen o oturumu devam ettir.
        //This, bu oturumu temsil etmektedir.
        else
        {
            instance = this;//5-
            DontDestroyOnLoad(gameObject); //6- Bu oturumu kapatma dedik.
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //7-PhotonNetworkten girdi�imiz �ifreyle ba�lant�y� yap dedik.

    }

    public void CreateRoom(string roomName)
    {//8-Oda a�ma i�lemleri yapaca��z.

        PhotonNetwork.CreateRoom(roomName); //9- 
    }


    public void JoinRoom(string roomName)
    {//10- Odaya kat�lma i�lemlerini ger�ekle�tirecek.

        PhotonNetwork.JoinRoom(roomName);

    }

    [PunRPC] //18- Yeni bir sahneye ge�ti�imizi t�m kullan�c�lara duyuruyoruz.
    public void ChangeScene(string sceneName)
    {//11- Sahne de�i�ikli�i yapaca��z. SceneManagerde sahne de�i�ikli�inde, terketti�imiz sahnedeki bilgiler silinmekteydi.

        PhotonNetwork.LoadLevel(sceneName);
    }

    //12-Bu i�lemlerden sonra Play'e bas�p fonklar�n olu�mas�n� sa�layabiliriz.
    //Bu kodlar�n duyurulmas� i�in �zel Callback fonk kullanaca��z. Duyurmak -> Kullan�c� odaya giri� yapt� gibi.
}
