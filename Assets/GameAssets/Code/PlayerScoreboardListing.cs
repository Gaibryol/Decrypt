using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerScoreboardListing : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    public TMP_Text score;
    public Photon.Realtime.Player Player { get; private set; }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        playerName.text = player.NickName;
        score.text = GetPlayerScore();
        UpdateUIIndicators();

    }

    public void UpdatePlayerInfo()
    {
        score.text = GetPlayerScore();

    }

    private string GetPlayerScore()
    {
        return Player.CustomProperties.GetValueOrDefault("Score", "").ToString();
    }

    public void UpdateUIIndicators()
    {
        if (Player == PhotonNetwork.LocalPlayer)    // this is me!
        {
            playerName.color = Color.green;

        }
        else
        {
            playerName.color = Color.white;
        }
    }
}
