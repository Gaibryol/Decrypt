using UnityEngine;
using System.Collections.Generic;

public class WordsManager : MonoBehaviour
{
    public static WordsManager Instance;

	private string possibleWordLengths;
    private List<int> odds;
	private List<string> spawnedList;

    private void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
        }

		possibleWordLengths = "4,5,6";
		spawnedList = new List<string>();
    }

    private int GetNumOfLetters()
    {
        switch (possibleWordLengths)
		{
            case Constants.FourFiveSix:
                odds = new List<int>(){4,4,5,5,5,5,5,5,5,6,6,6,6,6};
                break;
            case Constants.ThreeFourFiveSix:
                odds = new List<int>(){3,3,4,4,4,5,5,5,5,5,5,5,6,6,6,6};
                break;
            case Constants.ThreeFourFiveSixSeven:
                odds = new List<int>(){3,3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6,7,7,7};
                break;
            case Constants.ThreeFourFiveSixSevenEight:
                odds = new List<int>(){3,3,3,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6,7,7,7,7,8,8};
                break;
            case Constants.FourFiveSixSeven:
                odds = new List<int>(){4,4,4,5,5,5,5,5,5,5,6,6,6,6,7,7};
                break;
            case Constants.FourFiveSixSevenEight:
                odds = new List<int>(){4,4,4,4,5,5,5,5,5,5,5,5,6,6,6,6,6,7,7,7,8,8};
                break;
            case Constants.FourFiveSixEight:
                odds = new List<int>(){4,4,4,4,5,5,5,5,5,5,5,5,6,6,6,6,6,8,8};
                break;
            case Constants.ThreeFourFiveSixEight:
                odds = new List<int>(){3,3,3,4,4,4,4,5,5,5,5,5,5,5,5,6,6,6,6,6,8,8};
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

        for(int i = 0; i < characters.Length - 1; i++)
        {
            int randomNum = Random.Range(i,characters.Length);
            tempCharacter = characters[randomNum];
            characters[randomNum] = characters[i];
            characters[i] = tempCharacter;
        }

        return new string(characters);
    }

    public List<string> GetScrambledWord()
    {
        string randomWord = GetRandomWord();
        string shuffledWord = Shuffle(randomWord);

        while (shuffledWord == randomWord)
        {
            shuffledWord = Shuffle(randomWord);
			shuffledWord = Shuffle(shuffledWord);
        }
        
        return new List<string>(){randomWord,shuffledWord};
    }

    public void ChangePossibleWordLength(string list)
	{
        possibleWordLengths = list;
    }
}
