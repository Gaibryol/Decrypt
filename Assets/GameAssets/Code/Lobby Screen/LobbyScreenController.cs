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

    private void Awake()
    {
        StartCoroutine(JoinLobby());
    }

    private void Start()
    {
        roomListings = new List<LobbyRoomListing>();
        roomListings.Clear();       
    }
    

    private IEnumerator JoinLobby()
    {
        yield return new WaitUntil(() => PhotonController.Instance.IsConnectedAndReady);
        PhotonController.Instance.JoinLobby();
    }

    

    public void CreateRoom()
    {
        PhotonController.Instance.NickName = nicknameInput.text;
        PhotonController.Instance.CreateRoom();

        createButton.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonController.Instance.NickName = nicknameInput.text;
        PhotonController.Instance.JoinRoom(joinInput.text);
    }

    public void JoinRoom(string roomName)
    {
        PhotonController.Instance.NickName = nicknameInput.text;
        PhotonController.Instance.JoinRoom(roomName);
    }

    public void LeaveLobby()
    {
        PhotonController.Instance.LeaveLobby();
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

    public override void OnJoinedLobby()
    {
        PhotonController.Instance.HideLoading();
    }

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
