using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public Photon.Realtime.Player Player { get; private set; }

    public void SetPlayerInfo(Photon.Realtime.Player player)
    {
        Player = player;
        _text.text = player.NickName;
    }
}
