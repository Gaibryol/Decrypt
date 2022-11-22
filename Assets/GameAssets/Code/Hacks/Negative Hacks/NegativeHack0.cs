using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NegativeHack0 : Hack
{
    public NegativeHack0()
    {
        description = "Only the bottom row can be seen";
        hackFunction = Constants.HackFunction.None;
        effectType =  Constants.EffectType.Falling;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;

    }

    public override void Initialize()
    {
        HacksManager.Instance.ShowAmount = 1;
        base.Initialize();
    }
}
