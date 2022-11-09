using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class LobbyRoomListing : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomCapacity;
    [SerializeField] private Button joinButton;

    public RoomInfo _roomInfo { get; private set; }

    private LobbyScreenController LobbyScreenController;

    public void SetRoomInfo(RoomInfo roomInfo, LobbyScreenController controller)
    {
        _roomInfo = roomInfo;
        roomName.text = roomInfo.Name.ToString();
        roomCapacity.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        LobbyScreenController = controller;
    }

    public void JoinRoom()
    {
        LobbyScreenController.JoinRoom(roomName.text);
    }
}
