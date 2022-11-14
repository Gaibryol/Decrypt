
using System.Collections.Generic;
using UnityEngine;

public class WordsList : MonoBehaviour
{
    public static WordsList Instance { get; private set; }
    private HashSet<string> HashWords = new HashSet<string>();
    private List<string> ThreeWordList = new List<string>();
    private List<string> FourWordList = new List<string>();
    private List<string> FiveWordList = new List<string>();
    private List<string> SixWordList = new List<string>();
    private List<string> SevenWordList = new List<string>();
    private List<string> EightWordList = new List<string>();

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        var textFile = Resources.Load<TextAsset>("new_english");
        foreach(string tempWord in textFile.text.Split("\n"))
        {
            string word = tempWord.Trim();
            HashWords.Add(word);
            if(word.Length == 3) ThreeWordList.Add(word);
            if(word.Length == 4) FourWordList.Add(word);
            if(word.Length == 5) FiveWordList.Add(word);
            if(word.Length == 6) SixWordList.Add(word);
            if(word.Length == 7) SevenWordList.Add(word);
            if(word.Length == 8) EightWordList.Add(word);
        }
    }
    public List<string> GetThreeLetterWords(){
        return ThreeWordList;
    }
    public List<string> GetFourLetterWords(){
        return FourWordList;
    }
    public List<string> GetFiveLetterWords(){
        return FiveWordList;
    }
    public List<string> GetSixLetterWords(){
        return SixWordList;
    }
    public List<string> GetSevenLetterWords(){
        return SevenWordList;
    }
    public List<string> GetEightLetterWords(){
        return EightWordList;
    }
    public bool IfContains(string word)
    {
        if(HashWords.Contains(word)){
            return true;
        }
        else
        {
            return false;
        }
    }
}
    
