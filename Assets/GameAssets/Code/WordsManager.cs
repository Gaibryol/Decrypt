using UnityEngine;
using System.Collections.Generic;

public class WordsManager : MonoBehaviour
{
    public static WordsManager Instance { get; private set; }
	private string possibleWordLengths;
    private List<int> odds;
	private List<string> spawnedList;

    private void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
        }
        InitVariables();
    }
    public void InitVariables(){
        possibleWordLengths = "3,4,5";
        spawnedList = new List<string>();
    }
    private int GetNumOfLetters()
    {
        switch (possibleWordLengths)
		{
            case Constants.ThreeFourFive:
                odds = new List<int>(){3,3,4,4,4,4,5,5,5,5,5,5};
                break;
            case Constants.FourFiveSix:
                odds = new List<int>(){4,4,4,5,5,5,5,5,5,6,6,6,6,6};
                break;
            case Constants.FiveSixSeven:
                odds = new List<int>(){5,5,5,5,6,6,6,6,7,7};
                break;
            case Constants.ThreeFiveSixSeven:
                odds = new List<int>(){3,3,3,5,5,5,5,5,6,6,6,6,7,7,7};
                break;
            case Constants.FourFiveSixSeven:
                odds = new List<int>(){4,4,4,5,5,5,5,5,5,5,6,6,6,6,7,7,7};
                break;
            case Constants.FiveSixSevenEight:
                odds = new List<int>(){5,5,5,5,5,6,6,6,6,6,7,7,7,8,8};
                break;
            case Constants.SixSeven:
                odds = new List<int>(){6,6,6,7,7};
                break;
            case Constants.Seven:
                odds = new List<int>(){7};
                break;
            case Constants.SevenEight:
                odds = new List<int>(){7,7,7,8,8};
                break;
            case Constants.Eight:
                odds = new List<int>(){8};
                break;
        }

        int randomNum = Random.Range(0,odds.Count);
        int numLetters = odds[randomNum];
        return numLetters;
    }

    private string GetRandomWord()
    {
        int numLetters = GetNumOfLetters();

        List<string> aList;
        switch(numLetters)
		{
            case 3:
                aList = Constants.ThreeLetterList;
                break;
            case 4:
                aList = Constants.FourLetterList;
                break;
            case 5:
                aList = Constants.FiveLetterList;
                break;
            case 6:
                aList = Constants.SixLetterList;
                break;
            case 7:
                aList = Constants.SevenLetterList;
                break;
            default:
                aList = Constants.EightLetterList;
                break;
        }

        int randomNum = Random.Range(0,aList.Count);
        string randomWord = aList[randomNum];

        while (spawnedList.Contains(randomWord))
        {
            randomNum = Random.Range(0,aList.Count);
            randomWord = aList[randomNum];
        }

        spawnedList.Add(randomWord);
        return randomWord;
    }

    private string Shuffle(string word)
    {
        char[] characters = word.ToCharArray();
        char tempCharacter;

        for(int i = 0; i < characters.Length; i++)
        {
            tempCharacter = characters[i];
            int randomIndex= Random.Range(i,characters.Length);
            characters[i] = characters[randomIndex];
            characters[randomIndex] = tempCharacter;
        }
        return new string(characters);
    }


    public List<string> GetScrambledWord()
    {
        bool running = true;
        string randomWord = GetRandomWord();
        string shuffledWord = null;
        while (running)
        {
            if(HacksManager.Instance.ActivatedA)
            {
                char firstLetter = randomWord[0];
                shuffledWord = firstLetter + Shuffle(Shuffle(randomWord.Substring(1)));
            }
            else if(HacksManager.Instance.ActivatedB){
                char lastLetter = randomWord[randomWord.Length-1];
                shuffledWord = Shuffle(Shuffle(randomWord.Substring(0,randomWord.Length-1)))+ lastLetter;
            }else{
                shuffledWord = Shuffle(Shuffle(randomWord));
            }
            if(shuffledWord != randomWord)
            {
                running = false;
            }
        }
        return new List<string>(){randomWord,shuffledWord};
    }

    public void ChangePossibleWordLength(string list)
	{
        possibleWordLengths = list;
    }
}
