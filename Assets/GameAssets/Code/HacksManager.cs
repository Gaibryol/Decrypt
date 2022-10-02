using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksManager : MonoBehaviour
{
    public List<string> possibleEarlyHacks = new List<string>(){"A","B","C","D","E","F","G","H","I"};
    public List<string> possibleLateHacks = new List<string>(){"J","K","L","M","N","O","P",};
    public List<string> ActivatedHacks = new List<string>();
    public static HacksManager Instance { get; private set; }

	[SerializeField] private GameController gameController;
	[SerializeField] private GameUIController gameUIController;

    #region Bool Variables
    public bool ActivatedA = false;
    public bool ActivatedB = false;
    public bool ActivatedC = false;
    public bool ActivatedD = false;
    public bool ActivatedE = false;
    public bool ActivatedF = false;
    public bool ActivatedG = false;
    public bool ActivatedH = false;
    public bool ActivatedI = false;
    #endregion 

    private void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
        }
    }

    public void AddEarlyHack(string hackLetter)
    {
        possibleEarlyHacks.Remove(hackLetter);
        ActivatedHacks.Add(hackLetter);
        switch(hackLetter)
        {
            case Constants.HackA://Finished
                if(possibleEarlyHacks.Contains("D"))
                {
                    possibleEarlyHacks.Remove("D");
                }
                possibleEarlyHacks.Remove("B");
                ActivatedA = true;
				gameUIController.CoverWords(7);
                break;
            case Constants.HackB://Finished
                if(possibleEarlyHacks.Contains("D"))
                {
                    possibleEarlyHacks.Remove("D");
                }
                possibleEarlyHacks.Remove("A");
                ActivatedB = true;
				gameUIController.CoverWords(7);
                break;
            case Constants.HackC://Finished
                possibleEarlyHacks.Remove("H");
                ActivatedC = true;
                break;
            case Constants.HackD://Finished
                ActivatedD = true;
				gameController.ChangeMaxLife(1);
				gameUIController.CoverWords(5);
                break;
            case Constants.HackE://Finished
                possibleEarlyHacks.Remove("G");
                possibleEarlyHacks.Remove("I");
                ActivatedE = true;
				gameController.SetDecryptAmount(4);
                break;
            case Constants.HackF://Able to view letters as they are falling
                ActivatedF = true;
				gameController.ChangeMaxLife(-1);
                break;
            case Constants.HackG:
                possibleEarlyHacks.Remove("E");//Right Click Instantly drops
                possibleEarlyHacks.Remove("I");
                ActivatedG = true;
				gameController.ChangeMaxLife(-1);
                break;
            case Constants.HackH://Finished
                possibleEarlyHacks.Remove("C");
                ActivatedH = true;
                break;
            case Constants.HackI://Finished
                possibleEarlyHacks.Remove("E");
                possibleEarlyHacks.Remove("G");
                ActivatedI = true;
				gameController.SetDecryptAmount(1);
				gameController.ChangeMaxLife(-2);
                break;  
        }
    }

    public void AddLateHack(string hackLetter)
    {
        possibleLateHacks.Remove(hackLetter);
        ActivatedHacks.Add(hackLetter);
        switch(hackLetter)
        {
            case Constants.HackJ:
                WordsManager.Instance.ChangePossibleWordLength("3,5,6,7");
				gameController.SetMultiplier(0.75f);
                break;
            case Constants.HackK:
                WordsManager.Instance.ChangePossibleWordLength("4,5,6,7");
				gameController.SetMultiplier(0.85f);
                break;
            case Constants.HackL:
                WordsManager.Instance.ChangePossibleWordLength("5,6,7,8");
				gameController.SetMultiplier(1.50f);
                break;
            case Constants.HackM:
                WordsManager.Instance.ChangePossibleWordLength("6,7");
				gameController.SetMultiplier(1.20f);
                break;
            case Constants.HackN:
                WordsManager.Instance.ChangePossibleWordLength("7");
				gameController.SetMultiplier(2.00f);
                break;
            case Constants.HackO:
                WordsManager.Instance.ChangePossibleWordLength("7,8");
				gameController.SetMultiplier(2.50f);
                break;
            case Constants.HackP:
                WordsManager.Instance.ChangePossibleWordLength("8");
				gameController.SetMultiplier(4.00f);
                break;
        }
    }

    public List<string> GenerateHacks(string stage)
    {
        bool running = true;
        string hack1 = null;
        string hack2 = null;
        List<string> possibleHacks;
        if(stage == "Early")
		{
            possibleHacks = possibleEarlyHacks;
        }
        else
        {
            possibleHacks = possibleLateHacks;
        }
        while(running)
        {
            hack1 = possibleHacks[Random.Range(0,possibleHacks.Count)];
            hack2 = possibleHacks[Random.Range(0,possibleHacks.Count)];
            if(hack1 != hack2) 
                running = false;
        }
        return new List<string>(){hack1,hack2};
    }

    public string GetDescription(string hackLetter)
    {
        switch(hackLetter)
        {
            case Constants.HackA:
                return Constants.DescriptionA;
            case Constants.HackB:
                return Constants.DescriptionB;
            case Constants.HackC:
                return Constants.DescriptionC;
            case Constants.HackD:
                return Constants.DescriptionD;
            case Constants.HackE:
                return Constants.DescriptionE;
            case Constants.HackF:
                return Constants.DescriptionF;
            case Constants.HackG:
                return Constants.DescriptionG;
            case Constants.HackH:
                return Constants.DescriptionH;
            case Constants.HackI:
                return Constants.DescriptionI;
            case Constants.HackJ:
                return Constants.DescriptionJ;
            case Constants.HackK:
                return Constants.DescriptionK;
            case Constants.HackL:
                return Constants.DescriptionL;
            case Constants.HackM:
                return Constants.DescriptionM;
            case Constants.HackN:
                return Constants.DescriptionN;
            case Constants.HackO:
                return Constants.DescriptionO;
            case Constants.HackP:
                return Constants.DescriptionP;
        }
        return "None";
    }
}
