using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack6 : Hack
{
    public Hack6()
    {
        description = "Right-click instantly drops the falling word|See 1 less row";
        removeHacks = new List<Hack>{Constants.Hack0,Constants.Hack4,Constants.Hack8};
    }
    public override void Initialize()
    {
        gameController.ChangeMaxLife(-1);
        gameUIController.DisplayAbility("RIGHT CLICK TO INSTANTLY DROP A WORD",0);
        base.Initialize();
    }
    public void RightClick()
    {
        gameController.TeleportWord();
    }
}
