using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] private Sprite usedSprite;

    public bool used = false;

    public void UseAbility(){
        this.GetComponent<Image>().sprite = usedSprite;
        used = true;
    }

}
