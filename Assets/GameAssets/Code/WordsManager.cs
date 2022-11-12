using UnityEngine;
using System.Collections.Generic;

public class WordsManager : MonoBehaviour
{
    public static WordsManager Instance { get; private set; }
	private List<int> possibleWordLengths;
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
        possibleWordLengths = new List<int>(){3,4,5};
        spawnedList = new List<string>();
    }

    private string GetRandomWord()
    {
        int numLetters = possibleWordLengths[Random.Range(0,possibleWordLengths.Count)];

        List<string> aList;
        switch(numLetters)
		{
            case 3:
                aList = WordsList.Instance.GetThreeLetterWords();
                break;
            case 4:
                aList = WordsList.Instance.GetFourLetterWords();
                break;
            case 5:
                aList = WordsList.Instance.GetFiveLetterWords();
                break;
            case 6:
                aList = WordsList.Instance.GetSixLetterWords();
                break;
            case 7:
                aList = WordsList.Instance.GetSevenLetterWords();
                break;
            default:
                aList = WordsList.Instance.GetEightLetterWords();
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

    public string Shuffle(string word)
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

    public string ScrambledWordWithout(int index,string word)
    {
        string removedString =  word.Remove(index,1);
        bool running = true;
        string shuffledWord = null;
        while (running)
        {
            shuffledWord = Shuffle(Shuffle(removedString));
            shuffledWord = shuffledWord.Insert(index,word[index].ToString());
            if(shuffledWord != word)
            {
                running = false;
            }
            if(WordsList.Instance.IfContains(shuffledWord))
            {
                running = true;
            }
                
        }
        return shuffledWord;
    }
    public List<string> GetScrambledWord()
    {
        bool running = true;
        string randomWord = GetRandomWord();
        string shuffledWord = null;
        while (running)
        {
            shuffledWord = Shuffle(Shuffle(randomWord));
            if(shuffledWord != randomWord)
            {
                running = false;
            }
            if(WordsList.Instance.IfContains(shuffledWord))
            {
                running = true;
            }
            
        }
        return new List<string>(){randomWord,shuffledWord};
    }

    public void AddWordLength(int lengthOfWord)
	{
        if(!possibleWordLengths.Contains(lengthOfWord)){
            possibleWordLengths.Add(lengthOfWord);
        }
    }
        public void RemoveWordLength(int lengthOfWord)
	{
        if(possibleWordLengths.Contains(lengthOfWord)){
            possibleWordLengths.Remove(lengthOfWord);
        }
    }
    public void ChangeWordLengths(List<int> wordLengths){
        possibleWordLengths = wordLengths;
    }
}
