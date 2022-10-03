
using System.Collections.Generic;
using UnityEngine;

public class Contains : MonoBehaviour
{
    public static Contains Instance { get; private set; }
    private HashSet<string> HashWords = new HashSet<string>();

    private void Awake() {
        if (Instance == null)
		{
            Instance = this;
        }
        var textFile = Resources.Load<TextAsset>("clean_words_alpha");
        foreach(string word in textFile.text.Split("\n"))
        {
            HashWords.Add(word.Trim());
        }
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
    
