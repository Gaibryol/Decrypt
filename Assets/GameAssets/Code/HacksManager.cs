using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksManager : MonoBehaviour
{
    public List<string> possibleEarlyHacks;
    public List<string> possibleLateHacks;
    public List<string> ActivatedHacks;
    public static HacksManager Instance { get; private set; }

	[SerializeField] private GameController gameController;
	[SerializeField] private GameUIController gameUIController;

    private int covered;

    #region Bool Variables
    public bool ActivatedA;
    public bool ActivatedB;
    public bool ActivatedC;
    public bool ActivatedD;
    public bool ActivatedE;
    public bool ActivatedF;
    public bool ActivatedG;
    public bool ActivatedH;
    public bool ActivatedI;
    #endregion 

    private void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
        }
        InitVariables();
    }

    public void InitVariables()
    {
        covered = 0;
        possibleEarlyHacks = new List<string>(){"A","B","C","D","E","F","G","H","I"};
        possibleLateHacks = new List<string>(){"J","K","L","M","N","O","P",};
        ActivatedHacks = new List<string>();
        ActivatedA = false;
        ActivatedB = false;
        ActivatedC = false;
        ActivatedD = false;
        ActivatedE = false;
        ActivatedF = false;
        ActivatedG = false;
        ActivatedH = false;
        ActivatedI = false;
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
                possibleLateHacks.Remove("J");
                ActivatedA = true;
                covered = 1;
                break;
            case Constants.HackB://Finished
                if(possibleEarlyHacks.Contains("D"))
                {
                    possibleEarlyHacks.Remove("D");
                }
                possibleEarlyHacks.Remove("A");
                possibleLateHacks.Remove("J");
                ActivatedB = true;
                covered = 1;
                break;
            case Constants.HackC://Finished
                possibleEarlyHacks.Remove("H");
                possibleEarlyHacks.Remove("E");
                ActivatedC = true;
                break;
            case Constants.HackD://Finished
                ActivatedD = true;
				gameController.ChangeMaxLife(2);
                covered = 3;
                break;
            case Constants.HackE://Finished
                possibleEarlyHacks.Remove("G");
                possibleEarlyHacks.Remove("F");
                possibleEarlyHacks.Remove("C");
                possibleEarlyHacks.Remove("I");
                ActivatedE = true;
                gameUIController.DisplayAbility("RIGHT CLICK TO DESTROY A WORD", 4);
				gameController.SetDecryptAmount(4);
                break;
            case Constants.HackF://Finished
                ActivatedF = true;
                possibleEarlyHacks.Remove("A");
                possibleEarlyHacks.Remove("E");
				gameController.ChangeMaxLife(-1);
                break;
            case Constants.HackG:
                possibleEarlyHacks.Remove("A");
                possibleEarlyHacks.Remove("E");//Right Click Instantly drops
                possibleEarlyHacks.Remove("I");
                ActivatedG = true;
				gameController.ChangeMaxLife(-1);
                gameUIController.DisplayAbility("RIGHT CLICK TO INSTANTLY DROP A WORD",0);
                break;
            case Constants.HackH://Finished
                possibleEarlyHacks.Remove("C");
                gameController.ChangeMaxLife(-3);
                ActivatedH = true;
                break;
            case Constants.HackI://Finished
                possibleEarlyHacks.Remove("E");
                possibleEarlyHacks.Remove("G");
                ActivatedI = true;
                gameUIController.DisplayAbility("RIGHT CLICK TO CLEAR BOARD",1);
				gameController.SetDecryptAmount(1);
				gameController.ChangeMaxLife(-2);
                break;  
        }
		gameUIController.DisplayBaseline();
        if(covered != 0){
            gameUIController.CoverWords(covered);
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

		gameUIController.DisplayBaseline();
	}

    public List<string> GenerateHacks(int stage)
    {
        bool running = true;
        string hack1 = null;
        string hack2 = null;
        List<string> possibleHacks;
        if(stage != 3)
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
