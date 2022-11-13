using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonServerManager : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public static PhotonServerManager Instance { get; private set; }

    public Constants.PhotonState PhotonState;

    private LoadBalancingClient loadBalancingClient;
    private AppSettings appSettings;

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
        PhotonState = Constants.PhotonState.Disconected;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonPeer.RegisterType(typeof(GamePrefs), (byte)'M', GamePrefs.Serialize, GamePrefs.Deserialize);
        loadBalancingClient = PhotonNetwork.NetworkingClient;
        appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        loadBalancingClient.AddCallbackTarget(this);
    }

    private void Start()
    {
        PhotonController.Instance.ShowLoading();
    }

    public override void OnConnectedToMaster()
    {
        PhotonController.Instance.HideLoading();
        PhotonState = Constants.PhotonState.Connected;
    }

    /// <summary>
    /// WIP
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (CanRecoverFromDisconnect(cause))
        {
            TryReconnect();
        }
    }

    private bool CanRecoverFromDisconnect(DisconnectCause cause)
    {
        switch (cause)
        {
            // the list here may be non exhaustive and is subject to review
            case DisconnectCause.Exception:
            case DisconnectCause.ServerTimeout:
            case DisconnectCause.ClientTimeout:
            case DisconnectCause.DisconnectByServerLogic:
            case DisconnectCause.DisconnectByServerReasonUnknown:
                return true;
        }
        return false;
    }

    private void TryReconnect()
    {
        if (!loadBalancingClient.ReconnectAndRejoin())
        {
            Debug.LogError("ReconnectAndRejoin failed, trying Reconnect");
            if (!loadBalancingClient.ReconnectToMaster())
            {
                Debug.LogError("Reconnect failed, trying ConnectUsingSettings");
                if (!loadBalancingClient.ConnectUsingSettings(appSettings))
                {
                    Debug.LogError("ConnectUsingSettings failed");
                }
            }
        }
    }


}
