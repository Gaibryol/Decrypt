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

    private void Awake()
    {
        UI = GetComponent<RoomScreenUI>();
        PhotonController.Instance.UpdatePlayerState("PlayerState", "Room");
        GetRoomPlayers();
    }

    private void Start()
    {
        UI.UpdateRoomName($"Room Name: {PhotonNetwork.CurrentRoom.Name}");

        // Set scene sync to true for when master clicks start
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!PlayersReady()) return;

        SetPreferences();
        GameManager.Instance.ChangeState(Constants.GameStates.Game);
        
    }

    private bool PlayersReady()
    {
        return listings.Find(x => x.state.text != "Room") == null ? true : false;
    }
    
    public void SetPreferences()
    {
        // Set a new seed each game instance
        GameManager.Instance.GamePrefs.Seed = System.Environment.TickCount;
        PhotonController.Instance.UpdateRoomProperties("GamePrefs", GameManager.Instance.GamePrefs);
    }


    public void GetRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
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
        
        // Update UI indicators and enable UI buttons for master client
        listings.Find(x => x.Player == PhotonNetwork.MasterClient).UpdateUIIndicators();
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
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
}
