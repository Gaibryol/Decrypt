using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    public static PhotonServerManager Instance { get; private set; }

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

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonPeer.RegisterType(typeof(GamePrefs), (byte)'M', GamePrefs.Serialize, GamePrefs.Deserialize);
    }

    private void Start()
    {
        PhotonController.Instance.ShowLoading();
    }

    public override void OnConnectedToMaster()
    {
        PhotonController.Instance.HideLoading();
    }

    public bool JoinLobby()
    {
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.InLobby) return false;

        PhotonController.Instance.ShowLoading();
        PhotonNetwork.JoinLobby();
        return true;
    }

    public override void OnJoinedLobby()
    {
        PhotonController.Instance.HideLoading();
    }
}
