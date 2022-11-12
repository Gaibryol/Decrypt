using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//"The first letter of each word is in the correct position|Only the bottom row can be seen";
public class Hack0 : Hack
{
    public Hack0()
    {
        description = "The first letter of each word is in the correct position|Only the bottom row can be seen";
        removeHacks = new List<Hack>{HacksManager.Instance.Hack1,HacksManager.Instance.Hack3};
    }

    public override void Initialize()
    {
        HacksManager.Instance.ShowAmount = 1;
        base.Initialize();
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
