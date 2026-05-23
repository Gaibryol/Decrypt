using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRoyalHacksManager : HacksManager
{
    public List<Hack> PossibleNegativeHacks;
    public List<Hack> PossiblePositiveHacks;

    public Hack20 Hack20;
    public Hack30 Hack30;

    protected override void Awake()
    {
        if (GameManager.Instance.PlayMode == Constants.PlayMode.Single)
        {
            Destroy(this);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
        }
        InitVariables();
    }

    public override void InitVariables()
    {
        Hack20 = new Hack20();
        Hack30 = new Hack30();
        ShowAmount = 0;
        PossiblePositiveHacks = new List<Hack>() { Hack20 };
        PossibleNegativeHacks = new List<Hack>() { Hack30 };
        ActivatedHacks = new List<Hack>();
        PossibleHacks = new List<Hack>();
        PossibleHacks.AddRange(PossiblePositiveHacks);
        PossibleHacks.AddRange(PossibleNegativeHacks);
    }

    protected override void Update()
    {
        
    }
    public override void AddHack(string hack)
    {
        Hack h = PossibleHacks.Find(x => x.GetType().Name == hack);
        h.Initialize();
    }

    public override List<Hack> GenerateHacks()
    {
        Hack negativeHack = PossibleNegativeHacks[Random.Range(0, PossibleNegativeHacks.Count - 1)];
        Hack positiveHack = PossiblePositiveHacks[Random.Range(0, PossiblePositiveHacks.Count - 1)]; ;

        return new List<Hack>() { negativeHack, positiveHack };
    }
}
