using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject LoadingScreen;

    private GameObject activeLoadingScreen;

    public static PhotonController Instance { get; private set; }

    public List<Player> players = new List<Player>();

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
        activeLoadingScreen.SetActive(false);
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
}
