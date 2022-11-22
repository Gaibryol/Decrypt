using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayHack : MonoBehaviour
{
    [SerializeField] private TMP_Text positive;
    [SerializeField] private TMP_Text negative;

    public void SetDiplayHack(CombinedHack combinedHack){
        string[] description = combinedHack.GetDescription().Split("|");
        positive.text = description[0];
        negative.text = description[1];
    }
}
