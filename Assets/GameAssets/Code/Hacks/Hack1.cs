using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//"The last letter of each word is in the correct position|Only the bottom row can be seen";

public class Hack1 : Hack
{
    public Hack1()
    {
        description = "The last letter of each word is in the correct position|Only the bottom row can be seen";
        removeHacks = new List<Hack>{Constants.Hack0,Constants.Hack4};
    }
    public override void Initialize()
        {
        HacksManager.Instance.ShowAmount = 1;
        base.Initialize();
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
