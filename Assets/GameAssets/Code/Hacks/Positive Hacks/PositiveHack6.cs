using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack6 : Hack
{
    public PositiveHack6()
    {
        description = "Right-click instantly drops the falling word";
        hackFunction = Constants.HackFunction.RightClick;
        effectType =  Constants.EffectType.RightClick;
        rightClickTarget = Constants.RightClickTarget.All;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Initialize()
    {
        gameUIController.DisplayAbility("RIGHT CLICK TO INSTANTLY DROP A WORD",0);
        base.Initialize();
    }
    public override void RightClick(GameObject wordGameObject)
    {
        gameController.TeleportWord();
    }

}
