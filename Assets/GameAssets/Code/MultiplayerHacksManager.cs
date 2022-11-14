using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultiplayerHacksManager : HacksManager
{
    public List<Hack> PossibleNegativeHacks;
    public List<Hack> PossiblePositiveHacks;

    public override void Awake()
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
        ShowAmount = 0;
        PossiblePositiveHacks = new List<Hack>() { Constants.Hack20 };
        PossibleNegativeHacks = new List<Hack>() { Constants.Hack30 };
        ActivatedHacks = new List<Hack>();
        PossibleHacks = new List<Hack>();
        PossibleHacks.AddRange(PossiblePositiveHacks);
        PossibleHacks.AddRange(PossibleNegativeHacks);
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
