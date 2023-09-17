using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Butonlarla iþlemler yapacaðýmýz için
using TMPro; //Text Mesh Pro için
using Photon.Pun;
using Photon.Realtime; //Realtime iþlemler yapacaðýmýz için

public class Menu : MonoBehaviourPunCallbacks
{
    //1-
    [Header("Screens")] //Özelliklerde baþlýk baþlýk görünmesi için. Screen baþlýðý altýnda gameobjectler istenecek.
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    //2-
    [Header("Main Screen")] //Özeliklerde Main Screen adýnda baþlýk açtýk.
    public Button createRoomButton;
    public Button joinRoomButton;

    //3-
    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText; //Oyuncular listelenecek
    public Button startGameButton;

    private void Start()
    {//4- Ýlk baþta Pun'a baðlý olmadýðýnda create ve join butonlar aktif olmasýn dedik.
     //Bunlarý ilk baþta false baþlattýk. Oturum açýldýðýnda true olacak.
        createRoomButton.interactable = false; //Ýnteraktif etkileþim olabilir mi kýsmýný false yaptýk.
        joinRoomButton.interactable = false;
    }

    public override void OnConnectedToMaster() //5-
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true; //6- Baðlantý olduðundan true olacak dedik.
    }

    //7- Ekrandaki enable disable iþlemlerini yapalým.
    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        screen.SetActive(true); //Belirlediðimiz objeyi açtýk.
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {//8- Burada oda ismi ne yazacaksa
        NetworkManager.instance.CreateRoom(roomNameInput.text); //Odanýn ismi burada gelecek.
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {//9- Odaya katýlma iþlemi
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {//10- Oyuncu ismi sürekli update edilsin.
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom()
    {//11- Herhangi bir katýlýmcý dahil olduðunda mainScreen kapanacak. LobbyScreen açýlacak.
        SetScreen(lobbyScreen);//11
        photonView.RPC("UpdateLobbyUI", RpcTarget.All); //15- Lobiden çýkýþ olursa vs. Lobi herkese duyurulmuþ olacak.
    }

    [PunRPC] //Duyurma yaptýk.
    public void UpdateLobbyUI()
    {//12- Kullanýcýlar geldikçe lobi güncellensin. Lobi tamamlanýnca da kullanýcýlara duyurulsun.
        playerListText.text = ""; // Listemizi her seferinde temizleyerek Update ediyoruz.

        foreach (Player player in PhotonNetwork.PlayerList)
        {//13- Bütün katýlýmcýlar dahil olduðunda PlayerListe ekleniyor. Bizde her bir playera eriþtik.
            playerListText.text += player.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient)
        {//14- Eðer kiþi MasterClient ise
            startGameButton.interactable = true; //Start butonu MasterClientte aktif gözükecek.
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {//17- Herhangi bir kullanýcý ayrýlýrsa
        UpdateLobbyUI(); //Liste güncellenecek. Yani UI kýsmý güncellenecek.
    }

    public void OnLeaveLobbyButton()
    {//16- Katýlýmcýlar lobiden ayrýlmak isterse,
        PhotonNetwork.LeaveRoom(); //Odadan çýkmasýný istedik.
        SetScreen(mainScreen); //Tekrar giriþe yönlendirdik.
    }

    public void OnStartGameButton()
    {//18- Oyun baþladýðýnda herkes için sahne deðiþecek ve oyun ekranýna gidilecek.
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

    //19- Ýçerisinde RPC iþlemi yapýlan her objeye özellik olarak Photon View eklemeliyiz.
}
