using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PositiveHack1 : Hack
{
    public PositiveHack1()
    {
        description = "The last letter of each word is in the correct position";
        hackFunction = Constants.HackFunction.Apply;
        effectType =  Constants.EffectType.Reveal;
        rightClickTarget = Constants.RightClickTarget.None;
        gameMode = Constants.GameMode.Both;
    }
    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        int index = 0;
        int correctLetterIndex = word.realWord.Length-1;
        string shuffledWord =  WordsManager.Instance.ScrambledWordWithout(correctLetterIndex,word.realWord);
        word.scrambledWord = shuffledWord;
        
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
