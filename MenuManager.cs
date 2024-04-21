using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField nickName;
    public static string Nick;

    // Start is called before the first frame update
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions, TypedLobby.Default);
    }
    public void JoinRoom()
    {
            PhotonNetwork.JoinRoom(joinInput.text);
    }
    public override void OnJoinedRoom()
    {
            PhotonNetwork.LoadLevel("GANG");
    }

    public void NickName()
    {
        Nick = nickName.text;
        PhotonNetwork.NickName = Nick;

        print("Ваш ник: " + Nick);
    }
}
