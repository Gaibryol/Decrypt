using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksManager : MonoBehaviour
{
    public List<Hack> PossibleHacks;
    public List<Hack> ActivatedHacks;
    public int ShowAmount;
    public static HacksManager Instance { get; private set; }

	//[SerializeField] private GameController gameController;
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
        ShowAmount = 0;
        PossibleHacks = new List<Hack>(){Constants.Hack0,Constants.Hack1,Constants.Hack2,Constants.Hack3,Constants.Hack4,Constants.Hack5,Constants.Hack6,Constants.Hack7,Constants.Hack7};
        foreach(Hack tempHack in PossibleHacks)
        {
            tempHack.Deactivate();
        }
        ActivatedHacks = new List<Hack>();
    }
    private void Update(){
        Constants.Hack2.Update();
        Constants.Hack7.Update();
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
