using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//"The first letter of each word is in the correct position;
public class PositiveHack0 : Hack
{
    public PositiveHack0()
    {
        description = "The first letter of each word is in the correct position";
        hackFunction = Constants.HackFunction.Apply;
        effectType =  Constants.EffectType.Reveal;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.Both;
    }
    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        int correctLetterIndex = 0;
        string shuffledWord =  WordsManager.Instance.ScrambledWordWithout(correctLetterIndex,word.realWord);
        word.scrambledWord = shuffledWord;
        int index = 0;
		foreach (Transform obj in wordGameObject.transform)
		{
			obj.GetComponent<Letter>().ReplaceLetter(shuffledWord[index].ToString());
            if(index == correctLetterIndex){
                obj.GetComponent<Letter>().ChangeToCorrect();

            }
            index +=1;
		}
        
    }

}
