using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Butonlarla i�lemler yapaca��m�z i�in
using TMPro; //Text Mesh Pro i�in
using Photon.Pun;
using Photon.Realtime; //Realtime i�lemler yapaca��m�z i�in

public class Menu : MonoBehaviourPunCallbacks
{
    //1-
    [Header("Screens")] //�zelliklerde ba�l�k ba�l�k g�r�nmesi i�in. Screen ba�l��� alt�nda gameobjectler istenecek.
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    //2-
    [Header("Main Screen")] //�zeliklerde Main Screen ad�nda ba�l�k a�t�k.
    public Button createRoomButton;
    public Button joinRoomButton;

    //3-
    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText; //Oyuncular listelenecek
    public Button startGameButton;

    private void Start()
    {//4- �lk ba�ta Pun'a ba�l� olmad���nda create ve join butonlar aktif olmas�n dedik.
     //Bunlar� ilk ba�ta false ba�latt�k. Oturum a��ld���nda true olacak.
        createRoomButton.interactable = false; //�nteraktif etkile�im olabilir mi k�sm�n� false yapt�k.
        joinRoomButton.interactable = false;
    }

    public override void OnConnectedToMaster() //5-
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true; //6- Ba�lant� oldu�undan true olacak dedik.
    }

    //7- Ekrandaki enable disable i�lemlerini yapal�m.
    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        screen.SetActive(true); //Belirledi�imiz objeyi a�t�k.
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {//8- Burada oda ismi ne yazacaksa
        NetworkManager.instance.CreateRoom(roomNameInput.text); //Odan�n ismi burada gelecek.
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {//9- Odaya kat�lma i�lemi
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {//10- Oyuncu ismi s�rekli update edilsin.
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom()
    {//11- Herhangi bir kat�l�mc� dahil oldu�unda mainScreen kapanacak. LobbyScreen a��lacak.
        SetScreen(lobbyScreen);//11
        photonView.RPC("UpdateLobbyUI", RpcTarget.All); //15- Lobiden ��k�� olursa vs. Lobi herkese duyurulmu� olacak.
    }

    [PunRPC] //Duyurma yapt�k.
    public void UpdateLobbyUI()
    {//12- Kullan�c�lar geldik�e lobi g�ncellensin. Lobi tamamlan�nca da kullan�c�lara duyurulsun.
        playerListText.text = ""; // Listemizi her seferinde temizleyerek Update ediyoruz.

        foreach (Player player in PhotonNetwork.PlayerList)
        {//13- B�t�n kat�l�mc�lar dahil oldu�unda PlayerListe ekleniyor. Bizde her bir playera eri�tik.
            playerListText.text += player.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient)
        {//14- E�er ki�i MasterClient ise
            startGameButton.interactable = true; //Start butonu MasterClientte aktif g�z�kecek.
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {//17- Herhangi bir kullan�c� ayr�l�rsa
        UpdateLobbyUI(); //Liste g�ncellenecek. Yani UI k�sm� g�ncellenecek.
    }

    public void OnLeaveLobbyButton()
    {//16- Kat�l�mc�lar lobiden ayr�lmak isterse,
        PhotonNetwork.LeaveRoom(); //Odadan ��kmas�n� istedik.
        SetScreen(mainScreen); //Tekrar giri�e y�nlendirdik.
    }

    public void OnStartGameButton()
    {//18- Oyun ba�lad���nda herkes i�in sahne de�i�ecek ve oyun ekran�na gidilecek.
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

    //19- ��erisinde RPC i�lemi yap�lan her objeye �zellik olarak Photon View eklemeliyiz.
}
