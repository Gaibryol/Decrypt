using UnityEngine;
using System.Collections;

public class SinglePlayerGameController : GameController
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        StartGame();
    }

    public override void StartGame()
    {
        InitVariables();
        gameUI.StartGame();
    }

    protected override void InitVariables()
    {
        base.InitVariables();
    }
}
