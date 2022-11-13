using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class PhotonController : MonoBehaviourPunCallbacks
{
    public bool IsMaster { get { return PhotonNetwork.IsMasterClient; } }
    public bool InLobby { get { return PhotonNetwork.InLobby && PhotonNetwork.IsConnectedAndReady; } }
    public bool IsConnectedAndReady { get { return PhotonNetwork.IsConnectedAndReady; } }
    public string NickName { get { return PhotonNetwork.NickName; } set { PhotonNetwork.NickName = value; } }
    public string RoomName { get { return PhotonNetwork.CurrentRoom.Name; } }
    public bool AutoSyncScene { get { return PhotonNetwork.AutomaticallySyncScene; } set { PhotonNetwork.AutomaticallySyncScene = value; } }
    public Player[] Players { get { return PhotonNetwork.PlayerList; } }
    public Player Master { get { return PhotonNetwork.MasterClient; } private set { PhotonNetwork.SetMasterClient(value); } }
    public Player Me { get { return PhotonNetwork.LocalPlayer; } }
    public double Time { get { return PhotonNetwork.Time; } }

    [SerializeField] GameObject LoadingScreen;

    private GameObject activeLoadingScreen;

    public static PhotonController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ShowLoading()
    {
        if (activeLoadingScreen == null)
        {
            activeLoadingScreen = Instantiate(LoadingScreen);
        } else
        {
            activeLoadingScreen.SetActive(true);
        }
    }

    public void HideLoading()
    {
        if (activeLoadingScreen == null)
        {
            return;
        }
        activeLoadingScreen.SetActive(false);
    }

    //public Player UpdateMasterClient()
    //{
    //    if (!PhotonNetwork.IsMasterClient) return null;

    //    Player[] players = PhotonNetwork.PlayerList;

    //    if (players.Length < 2) return null;

    //    Player newMaster = players.First(x => x != PhotonNetwork.LocalPlayer);

    //    PhotonNetwork.SetMasterClient(newMaster);
    //    return newMaster;
    //}

    public void SetNextScene(string scene)
    {
        ExitGames.Client.Photon.Hashtable setScene = new ExitGames.Client.Photon.Hashtable();
        setScene["curScn"] = scene;
        PhotonNetwork.CurrentRoom.SetCustomProperties(setScene);
    }

    public void AddCallbackTarget(object target)
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void RemoveCallbackTarget(object target)
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void JoinLobby()
    {
        ShowLoading();
        PhotonNetwork.JoinLobby();
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;
        roomOptions.EmptyRoomTtl = 0;
        roomOptions.PlayerTtl = 1000;

        string roomName = $"{NickName}'s Room";
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SendPhotonEvent(byte eventCode, object content, ReceiverGroup receiverGroup)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup };
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPhotonEvent(byte eventCode, object content, int[] target)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, TargetActors = target };
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public bool AllPlayersInState(string state)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string playerState = player.CustomProperties["PlayerState"]?.ToString();
            if (playerState != state)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdatePlayerState(string key, object state)
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        if (props.ContainsKey(key))
        {
            props[key] = state;
        } else
        {
            props.Add(key, state);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void UpdateRoomProperties(string key, object state)
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        if (props.ContainsKey(key))
        {
            props[key] = state;
        }
        else
        {
            props.Add(key, state);
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}
