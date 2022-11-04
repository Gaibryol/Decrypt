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
    [SerializeField] private GameObject _masterIcon;
    public TMP_Text state;
    public Photon.Realtime.Player Player { get; private set; }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _text.text = player.NickName;
        state.text = GetPlayerState();
        UpdateUIIndicators();
        
    }

    public void UpdatePlayerInfo()
    {
        state.text = GetPlayerState();

    }

    private string GetPlayerState()
    {
        return Player.CustomProperties.GetValueOrDefault("PlayerState", "").ToString();
    }

    public void UpdateUIIndicators()
    {
        if (Player == PhotonNetwork.LocalPlayer)    // this is me!
        {
            _text.color = Color.green;
            
        } else
        {
            _text.color = Color.white;
        }
        if (Player.IsMasterClient)
        {
            _masterIcon.SetActive(true);
        } else
        {
            _masterIcon.SetActive(false);
        }
    }
}
