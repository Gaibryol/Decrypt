using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack
{
    protected HacksManager HM;
    protected GameController gameController;
    protected GameUIController gameUIController;
    protected Constants.HackFunction hackFunction;
    protected Constants.EffectType effectType;
    protected Constants.RightClickTarget rightClickTarget;
    protected Constants.GameMode gameMode;
    protected string description;
    protected int wordLength;



    public Hack()
    {
        HM = HacksManager.Instance;
        gameController = GameObject.Find("GameScreen").GetComponent<GameController>();
        gameUIController = GameObject.Find("GameScreen").GetComponent<GameUIController>();
    }
    public virtual void Initialize(){
        if(hackFunction == Constants.HackFunction.Apply)
        {
            HM.AddApplyHack(this);
        }
        else if(hackFunction == Constants.HackFunction.Update)
        {
            HM.SetUpdateHack(this);
        }
        else if(hackFunction == Constants.HackFunction.RightClick)
        {
            HM.SetRightClick(this);
        }
        gameUIController.DisplayBaseline();
        gameUIController.CoverWords(HacksManager.Instance.ShowAmount);
    }

    public virtual void Update()
    {

    }
    public virtual void RightClick(GameObject wordGameObject)
    {
        
    }
    public virtual void Apply(GameObject wordGameObject)
    {
        
    }
    public string GetDescription()
    {
        return description;
    }
    public Constants.EffectType GetEffectType()
    {
        return effectType;
    }

    public Constants.HackFunction GetHackFunction()
    {
        return hackFunction;
    }
    public Constants.RightClickTarget GetRightClickType()
    {
        return rightClickTarget;
    }
    public int GetWordLength()
    {
        return wordLength;
    }
    public Constants.GameMode GetGameMode()
    {
        return gameMode;
    }





}
