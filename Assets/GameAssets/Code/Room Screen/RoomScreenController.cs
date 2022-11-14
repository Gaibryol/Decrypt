using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class RoomScreenController : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing playerListingPrefab;
    [SerializeField] private Transform playerListingMenu;

    private List<PlayerListing> listings = new List<PlayerListing>();
    private RoomScreenUI UI;

    private int battleRoyalMinPlayers = 2;

    private void Awake()
    {
        UI = GetComponent<RoomScreenUI>();
        PhotonController.Instance.UpdatePlayerState("PlayerState", "Room");
        GetRoomPlayers();
    }

    private void Start()
    {
        UI.UpdateRoomName($"Room Name: {PhotonController.Instance.RoomName}");

        // Set scene sync to true for when master clicks start

        //PhotonController.Instance.AutoSyncScene = true;

        GameManager.Instance.GamePrefs.Seed = System.Environment.TickCount;
        StartCoroutine(WaitForMaster());

    }

    private IEnumerator WaitForMaster()
    {
        yield return new WaitUntil(() => PhotonController.Instance.AllPlayersInState("Room"));
        PhotonController.Instance.AutoSyncScene = true;

    }

    public void StartGame()
    {
        if (!PhotonController.Instance.IsMaster) return;
        if (!PhotonController.Instance.AllPlayersInState("Room")) return;
        if ((GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR) && (PhotonController.Instance.Players.Length < battleRoyalMinPlayers)) return;

        SetPreferences();
        GameManager.Instance.ChangeState(Constants.GameStates.Game);
        
    }
    
    public void SetPreferences()
    {
        // Set a new seed each game instance
        GameManager.Instance.GamePrefs.Seed = System.Environment.TickCount;
        PhotonController.Instance.UpdateRoomProperties("GamePrefs", GameManager.Instance.GamePrefs);
    }


    public void GetRoomPlayers()
    {
        foreach (Player player in PhotonController.Instance.Players)
        {
            AddPlayerListing(player);
        }
    }

    public void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate(playerListingPrefab, playerListingMenu);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            listings.Add(listing);
        }
    }

    // <-------       Callbacks       ------->

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Destroy player listing, and remove from list
        int idx = listings.FindIndex(x => x.Player == otherPlayer);
        if (idx != -1)
        {
            Destroy(listings[idx].gameObject);
            listings.RemoveAt(idx);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Update UI indicators and enable UI buttons for master client
        listings.Find(x => x.Player == newMasterClient).UpdateUIIndicators();
        if (PhotonController.Instance.Me == newMasterClient)
        {
            UI.UpdateMasterButtons();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        listings.First(x => x.Player == targetPlayer).UpdatePlayerInfo();
    }

    public override void OnLeftRoom()
    {
        GameManager.Instance.ChangeState(Constants.GameStates.Lobby);
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        var obj = propertiesThatChanged["GamePrefs"];
        if (obj != null)
        {
            GameManager.Instance.GamePrefs = (GamePrefs)obj;
        }
    }

}
