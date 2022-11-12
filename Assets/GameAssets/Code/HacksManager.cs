using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksManager : MonoBehaviour
{
    public List<Hack> PossibleHacks;
    public List<Hack> ActivatedHacks;
    public int ShowAmount;
    public static HacksManager Instance { get; private set; }

    
	public Hack0 Hack0; //  "The first letter of each word is in the correct position|Only the bottom row can be seen";
	public Hack1 Hack1; //  "The last letter of each word is in the correct position|Only the bottom row can be seen";
	public Hack2 Hack2;//"Decrypt the longest words every 25 seconds|Each word has a chance of having a letter obscured";
	public Hack3 Hack3; // "Increase the maximum amount of rows by 2|Only the bottom three rows can be seen";
	public Hack4 Hack4; // "Right-click to decrypt a word instantly (Up to 4 uses in a game)|Letters only appear when hovering over the letter tile";
	public Hack5 Hack5; //"Letters can be seen as they’re falling down|See 1 less row";
	public Hack6 Hack6; // "Right-click instantly drops the falling word|See 1 less row";
	public Hack7 Hack7; // "Decrypt a random word every 25 seconds|See 3 less row";
	public Hack8 Hack8; //"Decrypt all words (One use per game)|See 2 less lines";

	[SerializeField] private GameController gameController;
	[SerializeField] private GameUIController gameUIController;




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
        Hack0= new Hack0();
        Hack1= new Hack1();
        Hack2= new Hack2();
        Hack3= new Hack3();
        Hack4= new Hack4();
        Hack5= new Hack5();
        Hack6= new Hack6();
        Hack7= new Hack7();
        Hack8= new Hack8();
        ShowAmount = 0;
        PossibleHacks = new List<Hack>(){Hack0,Hack1,Hack2,Hack3,Hack4,Hack5,Hack6,Hack7,Hack7};
        foreach(Hack tempHack in PossibleHacks)
        {
            tempHack.Deactivate();
        }
        ActivatedHacks = new List<Hack>();
    }
    private void Update(){
        Hack2.Update();
        Hack7.Update();
    }

    public void AddHack(Hack hack)
    {
        PossibleHacks.Remove(hack);
        ActivatedHacks.Add(hack);
        foreach(Hack tempHack in hack.GetRemoveHacks())
        {
            PossibleHacks.Remove(tempHack);
        }
        hack.Initialize();
	}

    public List<Hack> GenerateHacks()
    {
        bool running = true;
        Hack hack1 = null;
        Hack hack2 = null;

        while(running)
        {
            hack1 = PossibleHacks[Random.Range(0,PossibleHacks.Count-1)];
            hack2 = PossibleHacks[Random.Range(0,PossibleHacks.Count-1)];
            if(hack1 != hack2)
			{
				running = false;
			}
        }
        return new List<Hack>(){hack1,hack2};
    }
    public void Apply(GameObject wordGameObject){
        foreach(Hack hack in ActivatedHacks){
            hack.Apply(wordGameObject);
        }
    }
}
