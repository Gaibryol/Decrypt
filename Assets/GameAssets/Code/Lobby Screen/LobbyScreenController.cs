using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScreenController : MonoBehaviourPunCallbacks
{
    [Header("Buttons")]
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TMP_InputField nicknameInput;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        PhotonNetwork.NickName = nicknameInput.text;

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        createButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room Success");
        GameManager.Instance.ChangeState(Constants.GameStates.Room);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room Creation Failed: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Failed: {message}");

    }
}
