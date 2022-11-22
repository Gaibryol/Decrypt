using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NegativeHack3 : Hack
{
    public NegativeHack3()
    {
        description = "Letters only appear when hovering over the letter tile";
        hackFunction = Constants.HackFunction.Apply;
        effectType =  Constants.EffectType.Obscure;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.Both;
    }

    public override void Apply(GameObject wordGameObject){
        Word word = wordGameObject.GetComponent<Word>();
		foreach (Transform obj in wordGameObject.transform)
		{
            obj.GetComponent<Letter>().IsHover();
		}
    }
}
