using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject GamePreferences;

    public void ToggleGamePreferences()
    {
        GamePreferences.SetActive(!GamePreferences.activeInHierarchy);
    }
}
