using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack3 : Hack
{
    public PositiveHack3()
    {
        description = "Increase the maximum amount of rows by 2";
        hackFunction = Constants.HackFunction.None;
        effectType =  Constants.EffectType.Lines;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Initialize()
    {
        gameController.ChangeMaxLife(2);
        base.Initialize();
    }

}
