using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Web;
using System.Text;
using System.Linq;

public class LobbyScreenController : MonoBehaviourPunCallbacks
{
    [Header("Buttons")]
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;

    [Header("Inputs")]
    //[SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TMP_InputField nicknameInput;

    [Header("RoomListing")]
    [SerializeField] private LobbyRoomListing roomListingPrefab;
    [SerializeField] private Transform roomListingParent;

    private List<LobbyRoomListing> roomListings;

    private int roomCodeLength = 4;

    private bool isConnecting = false;

    private void Start()
    {
        roomListings = new List<LobbyRoomListing>();
        roomListings.Clear();
        //UpdateRooms();
        
    }
    private void Update()
    {
        if (!PhotonNetwork.InLobby && PhotonNetwork.IsConnectedAndReady && !isConnecting)
        {
            isConnecting = PhotonServerManager.Instance.JoinLobby();
        }
        //UpdateRooms();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;
        roomOptions.EmptyRoomTtl = 0;
        PhotonNetwork.NickName = nicknameInput.text;

        //string roomName = GenerateRoomName(roomCodeLength);
        string roomName = $"{nicknameInput.text}'s Room";
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        createButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.NickName = nicknameInput.text;
        PhotonNetwork.JoinRoom(roomName);
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            int idx = roomListings.FindIndex(x => x._roomInfo.Name == info.Name);
            if (info.RemovedFromList && idx != -1)
            {
                Destroy(roomListings[idx].gameObject);
                roomListings.RemoveAt(idx);
            }
            else if (idx == -1)
            {
                LobbyRoomListing listing = Instantiate(roomListingPrefab, roomListingParent);
                if (listing != null)
                {
                    roomListings.Add(listing);
                    listing.SetRoomInfo(info, this);
                }
            }
        }
    }
}
