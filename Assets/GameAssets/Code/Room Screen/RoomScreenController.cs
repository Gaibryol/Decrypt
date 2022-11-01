using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomScreenController : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing playerListingPrefab;
    [SerializeField] private Transform playerListingMenu;

    private List<PlayerListing> listings = new List<PlayerListing>();


    private void Awake()
    {
        GetRoomPlayers();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        SetPreferences();
        GameManager.Instance.ChangeState(Constants.GameStates.Game);
        
    }
    
    public void SetPreferences()
    {
        GameManager.Instance.GamePrefs.GameType = Constants.GameType.Timed;
        GameManager.Instance.GamePrefs.Seed = System.Environment.TickCount;
        //GameManager.Instance.GamePrefs.Timer = 30;
        //GameManager.Instance.GamePrefs.WordLengths = new List<int>() { 3, 4 };

        ExitGames.Client.Photon.Hashtable gamePrefs = new ExitGames.Client.Photon.Hashtable();
        gamePrefs.Add("GamePrefs", GameManager.Instance.GamePrefs);

        PhotonNetwork.CurrentRoom.SetCustomProperties(gamePrefs);
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
            PhotonController.Instance.players.Add(player);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        int idx = listings.FindIndex(x => x.Player == otherPlayer);
        if (idx != -1)
        {
            Destroy(listings[idx].gameObject);
            listings.RemoveAt(idx);
            PhotonController.Instance.players.Remove(otherPlayer);

        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room Successful");
    }
}
