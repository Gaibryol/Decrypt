using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack7 : Hack
{
    private float decryptTime;
    public PositiveHack7()
    {
        decryptTime = Constants.DecryptTime;
        description = "Decrypt a random word every 25 seconds";
        hackFunction = Constants.HackFunction.Update;
        effectType =  Constants.EffectType.DecryptTime;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Update() {
        if(gameController.GetSubState() == Constants.SubState.Playing)
            decryptTime -= Time.deltaTime;
        if (decryptTime <= 0)
        {
            gameController.DecryptRandomWord();
            decryptTime = Constants.DecryptTime;
        }
    }

}
