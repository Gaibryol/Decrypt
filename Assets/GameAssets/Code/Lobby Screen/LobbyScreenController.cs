using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScreenController : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerListing playerListingPrefab;
    [SerializeField] private Transform playerListingMenu;

    private List<PlayerListing> listings = new List<PlayerListing>();
    ExitGames.Client.Photon.Hashtable gamePrefs = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        GetRoomPlayers();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        gamePrefs.Add("GameType", Constants.GameType.Timed);
        gamePrefs.Add("Seed", System.Environment.TickCount);
        PhotonNetwork.CurrentRoom.SetCustomProperties(gamePrefs);
        PhotonNetwork.LoadLevel("GameScene");
        
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
        }
    }
}
