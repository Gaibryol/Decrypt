using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NegativeHack2 : Hack
{
    public NegativeHack2()
    {
        description = "Only the bottom three rows can be seen";
        hackFunction = Constants.HackFunction.None;
        effectType =  Constants.EffectType.Falling;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }

    public override void Initialize()
    {
        HacksManager.Instance.ShowAmount = 3;
        base.Initialize();
    }
}
