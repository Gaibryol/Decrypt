using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Web;
using System.Text;

public class LobbyScreenController : MonoBehaviourPunCallbacks
{
    [Header("Buttons")]
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;

    [Header("Inputs")]
    //[SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TMP_InputField nicknameInput;

    private int roomCodeLength = 4;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        PhotonNetwork.NickName = nicknameInput.text;

        string roomName = GenerateRoomName(roomCodeLength);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        createButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);
    }

    private string GenerateRoomName(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();

        while (0 < length--)
        {
            res.Append(valid[Random.Range(0, valid.Length)]);
        }
        return res.ToString();
    }
    
    // <-------       Callbacks       ------->
    public override void OnCreatedRoom()
    {
        
    }

    public override void OnJoinedRoom()
    {
        GameManager.Instance.ChangeState(Constants.GameStates.Room);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == 32766)    // Room of same name exists already
        {
            CreateRoom();
        }
        Debug.Log($"Room Creation Failed: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Failed: {message}");
    }
}
