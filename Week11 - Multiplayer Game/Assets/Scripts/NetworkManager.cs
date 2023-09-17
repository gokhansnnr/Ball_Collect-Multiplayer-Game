using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //1- Photon dahil ettik.

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;//2- Herbir oyuncunun katýlýmý için bir durum açmamýz lazým.
    //Oyunun açýlmasý özel bir iþlemdir. O sebeple static dedik.
    private void Awake()
    {//Lobi açma, room açma gibi iþlemleri burada yapýyoruz.
        //Awake, Starttan bir týk önce gerçekleþir. Oyun oluþmadan önce burasý çalýþýr.

        if (instance != null && instance != this) //3-Bir oturum açýlmýþsa (boþ deðilse) ve instance þuanki oturum deðilse (this),
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
        PhotonNetwork.ConnectUsingSettings(); //7-PhotonNetworkten girdiðimiz þifreyle baðlantýyý yap dedik.

    }

    public void CreateRoom(string roomName)
    {//8-Oda açma iþlemleri yapacaðýz.

        PhotonNetwork.CreateRoom(roomName); //9- 
    }


    public void JoinRoom(string roomName)
    {//10- Odaya katýlma iþlemlerini gerçekleþtirecek.

        PhotonNetwork.JoinRoom(roomName);

    }

    [PunRPC] //18- Yeni bir sahneye geçtiðimizi tüm kullanýcýlara duyuruyoruz.
    public void ChangeScene(string sceneName)
    {//11- Sahne deðiþikliði yapacaðýz. SceneManagerde sahne deðiþikliðinde, terkettiðimiz sahnedeki bilgiler silinmekteydi.

        PhotonNetwork.LoadLevel(sceneName);
    }

    //12-Bu iþlemlerden sonra Play'e basýp fonklarýn oluþmasýný saðlayabiliriz.
    //Bu kodlarýn duyurulmasý için özel Callback fonk kullanacaðýz. Duyurmak -> Kullanýcý odaya giriþ yaptý gibi.
}
