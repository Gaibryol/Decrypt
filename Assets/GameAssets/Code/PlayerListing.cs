using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public TMP_Text state;
    public Photon.Realtime.Player Player { get; private set; }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _text.text = player.NickName;
        state.text = GetPlayerState();
    }

    public void UpdatePlayerInfo()
    {
        state.text = GetPlayerState();
    }

    private string GetPlayerState()
    {
        string state = Player.CustomProperties.GetValueOrDefault("PlayerState", "").ToString();
        Debug.Log($"State: {state}");
        return state;
    }
}
