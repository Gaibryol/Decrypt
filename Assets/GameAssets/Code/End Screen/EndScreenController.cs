using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class EndScreenController : MonoBehaviourPunCallbacks
{
    List<PlayerScoreboardListing> playerListings;
    [SerializeField] private PlayerScoreboardListing playerListingPrefab;
    [SerializeField] private Transform playerListingMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerListings = new List<PlayerScoreboardListing>();
        UpdatePlayerList();
        PhotonController.Instance.SetNextScene("RoomScene");
        PhotonController.Instance.UpdatePlayerState("PlayerState", "Scoreboard");

    }

    private void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            PlayerScoreboardListing listing = Instantiate(playerListingPrefab, playerListingMenu);
            if (listing != null)
            {
                listing.SetPlayerInfo(players[i]);
                playerListings.Add(listing);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMainMenu()
    {
        SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");

        PhotonNetwork.LeaveRoom();

    }

    public void ToRoom()
    {
        GameManager.Instance.ChangeState(Constants.GameStates.Room);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);


    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        playerListings.First(x => x.Player == targetPlayer).UpdatePlayerInfo();
    }
}
