using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack5 : Hack
{
    public PositiveHack5()
    {
        description = "Letters can be seen as they’re falling down";
        hackFunction = Constants.HackFunction.Apply;
        effectType =  Constants.EffectType.Falling;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        foreach (Transform obj in wordGameObject.transform)
		{
			obj.GetComponent<Letter>().RevealLetter();
		}
    }

}
