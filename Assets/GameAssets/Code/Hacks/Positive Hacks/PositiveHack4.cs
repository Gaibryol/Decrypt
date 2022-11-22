using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack4 : Hack
{
    public PositiveHack4()
    {
        description = "Right-click to decrypt a word instantly (Up to 4 uses in a game)";
        hackFunction = Constants.HackFunction.RightClick;
        effectType =  Constants.EffectType.RightClick;
        rightClickTarget = Constants.RightClickTarget.Word;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Initialize()
    {
        gameUIController.DisplayAbility("RIGHT CLICK TO DESTROY A WORD", 4);
        gameController.SetDecryptAmount(4);
        base.Initialize();
    }

    public override void RightClick(GameObject wordGameObject)
    {
        gameController.DecryptWord(wordGameObject);
    }
}
