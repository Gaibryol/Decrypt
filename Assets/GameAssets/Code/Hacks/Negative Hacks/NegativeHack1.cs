using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NegativeHack1 : Hack
{
    public NegativeHack1()
    {
        description = "Each word has a chance of having a letter obscured";
        hackFunction = Constants.HackFunction.Apply;
        effectType =  Constants.EffectType.Obscure;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.SinglePlayer;
    }
    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        if(Random.Range(0,3) == 1){
            word.letters[Random.Range(0,word.letters.Count)].GetComponent<Letter>().NeverReveal();
        }
    }
}
