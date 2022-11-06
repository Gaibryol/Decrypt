using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class PhotonController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject LoadingScreen;

    private GameObject activeLoadingScreen;

    public static PhotonController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
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

    public Player UpdateMasterClient()
    {
        if (!PhotonNetwork.IsMasterClient) return null;

        Player[] players = PhotonNetwork.PlayerList;

        if (players.Length < 2) return null;

        Player newMaster = players.First(x => x != PhotonNetwork.LocalPlayer);

        PhotonNetwork.SetMasterClient(newMaster);
        return newMaster;
    }

    public void SetNextScene(string scene)
    {
        ExitGames.Client.Photon.Hashtable setScene = new ExitGames.Client.Photon.Hashtable();
        setScene["curScn"] = scene;
        PhotonNetwork.CurrentRoom.SetCustomProperties(setScene);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        var obj = propertiesThatChanged["GamePrefs"];
        if (obj != null)
        {
            GameManager.Instance.GamePrefs = (GamePrefs)obj;
            SendPhotonEvent(Constants.PlayerReadyEventCode, PhotonNetwork.LocalPlayer, ReceiverGroup.MasterClient);
        }
    }

    public void SendPhotonEvent(byte eventCode, object content, ReceiverGroup receiverGroup)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = receiverGroup };
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void UpdatePlayerState(string key, object state)
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        ExitGames.Client.Photon.Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
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

        ExitGames.Client.Photon.Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
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
