using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack2 : Hack
{
        private float decryptTime;
    public PositiveHack2()
    {
        decryptTime = Constants.DecryptTime;

        description = "Decrypt the longest words every 25 seconds";
        hackFunction = Constants.HackFunction.Update;
        effectType =  Constants.EffectType.DecryptTime;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Update(){
        if(gameController.GetSubState() == Constants.SubState.Playing)
            decryptTime -= Time.deltaTime;
        if (decryptTime <= 0)
        {
            gameController.DecryptLongestWord();
            decryptTime = Constants.DecryptTime;
        }
    }
}
