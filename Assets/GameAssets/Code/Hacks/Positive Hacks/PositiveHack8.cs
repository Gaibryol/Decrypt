using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack8 : Hack
{
    public PositiveHack8()
    {
        description = "Decrypt all words (One use per game)";
        hackFunction = Constants.HackFunction.RightClick;
        effectType =  Constants.EffectType.RightClick;
        rightClickTarget = Constants.RightClickTarget.All;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    
    public override void Initialize()
    {

        gameUIController.DisplayAbility("RIGHT CLICK TO CLEAR BOARD",1);
        gameController.SetDecryptAmount(1);
        base.Initialize();
    }

    public override void RightClick(GameObject wordGameObject)
    {
        gameController.DecryptList();
    }
}
