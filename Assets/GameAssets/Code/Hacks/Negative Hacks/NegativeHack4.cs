using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NegativeHack4 : Hack
{
    public NegativeHack4()
    {
        description = "Decrease the maximum amount of rows by 1";
        hackFunction = Constants.HackFunction.None;
        effectType =  Constants.EffectType.Lines;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }

    public override void Initialize()
    {
        gameController.ChangeMaxLife(-1);
        base.Initialize();
    }
}
