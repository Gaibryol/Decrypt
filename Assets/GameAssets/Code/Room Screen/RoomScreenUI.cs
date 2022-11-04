using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Subject to all be changed as UI updates.
/// </summary>
public class RoomScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject GamePreferences;
    [SerializeField] private GameObject GamePreferencesButton;
    [SerializeField] private Toggle DefaultButton;
    [SerializeField] private Toggle TimedButton;

    [SerializeField] private GameObject TimeLimitOption;
    [SerializeField] private GameObject CountDownTimeOption;
    [SerializeField] private GameObject WordLengthOption;

    [SerializeField] private GameObject StartButton;

    [SerializeField] private TMP_InputField TimeLimitInput;
    [SerializeField] private TMP_InputField CountDownInput;
    [SerializeField] private TMP_InputField WordLengthInput;

    [SerializeField] private TMP_Text RoomName;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
            GamePreferences.SetActive(false); // master can only edit?
            GamePreferencesButton.SetActive(false);
        }
    }

    public void ToggleGamePreferences()
    {
        GamePreferences.SetActive(!GamePreferences.activeInHierarchy);
        
        if (!GamePreferences.activeInHierarchy)
        {
            ParseInput();
        } else
        {
            FillInput();
        }
    }

    public void UpdateRoomName(string name)
    {
        RoomName.text = name;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            Player newMaster = PhotonController.Instance.UpdateMasterClient();
        }
    }

    public void DefaultMode()
    {
        WordLengthOption.SetActive(true);
        TimeLimitOption.SetActive(false);
        CountDownTimeOption.SetActive(false);   // not supported 
    }

    public void TimedMode()
    {
        WordLengthOption.SetActive(true);
        TimeLimitOption.SetActive(true);
        CountDownTimeOption.SetActive(false);   // not supported
    }

    public void UpdateMasterButtons()
    {
        StartButton.SetActive(true);
        GamePreferencesButton.SetActive(true);
    }

    private void ParseInput()
    {
        GameManager.Instance.GamePrefs.Timer = Int32.Parse(TimeLimitInput.text);
        List<int> wordLengths = WordLengthInput.text.Split(',').Select(i => Int32.Parse(i)).ToList();
        GameManager.Instance.GamePrefs.WordLengths = wordLengths;
        GameManager.Instance.GamePrefs.GameType = DefaultButton.isOn ? Constants.GameType.Default : Constants.GameType.Timed;

    }

    private void FillInput()
    {
        TimeLimitInput.text = GameManager.Instance.GamePrefs.Timer.ToString();
        WordLengthInput.text = string.Join(',', GameManager.Instance.GamePrefs.WordLengths);
        TimedButton.isOn = GameManager.Instance.GamePrefs.GameType == Constants.GameType.Timed ? true : false;
        
        if (DefaultButton.isOn)
        {
            DefaultMode();
        }
        else
        {
            TimedMode();
        }
    }
}
