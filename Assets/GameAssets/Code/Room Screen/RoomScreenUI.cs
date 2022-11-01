using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class RoomScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject GamePreferences;
    [SerializeField] private Button DefaultButton;
    [SerializeField] private Button TimedButton;

    [SerializeField] private GameObject TimeLimitOption;
    [SerializeField] private GameObject CountDownTimeOption;
    [SerializeField] private GameObject WordLengthOption;

    [SerializeField] private TMP_InputField TimeLimitInput;
    [SerializeField] private TMP_InputField CountDownInput;
    [SerializeField] private TMP_InputField WordLengthInput;

    public void ToggleGamePreferences()
    {
        GamePreferences.SetActive(!GamePreferences.activeInHierarchy);
        DefaultMode();
        if (!GamePreferences.activeInHierarchy)
        {
            ParseInput();
        } else
        {
            FillInput();
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

    private void ParseInput()
    {
        GameManager.Instance.GamePrefs.Timer = Int32.Parse(TimeLimitInput.text);
        List<int> wordLengths = WordLengthInput.text.Split(',').Select(i => Int32.Parse(i)).ToList();
        GameManager.Instance.GamePrefs.WordLengths = wordLengths;

    }

    private void FillInput()
    {
        TimeLimitInput.text = GameManager.Instance.GamePrefs.Timer.ToString();
        WordLengthInput.text = string.Join(',', GameManager.Instance.GamePrefs.WordLengths);
    }
}
