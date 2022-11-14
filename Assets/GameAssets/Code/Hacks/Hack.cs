using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack:IHack
{
    protected string description;
    protected List<Hack> removeHacks;
    protected GameController gameController;
    protected GameUIController gameUIController;

    protected HacksManager HM;

    public bool activated;
    public Hack()
    {
        HM = HacksManager.Instance;
        description = "";
        removeHacks = new List<Hack>{};
        gameController = GameObject.Find("GameScreen").GetComponent<GameController>();
        gameUIController = GameObject.Find("GameScreen").GetComponent<GameUIController>();
        activated = false;
        
    }
    public virtual void Initialize(){
        gameUIController.DisplayBaseline();
        gameUIController.CoverWords(HacksManager.Instance.ShowAmount);
        activated = true;
    }
    public virtual void Apply(GameObject wordGameObject)
    {
        
    }
    public void Deactivate()
    {
        activated = false;
    }

    public string GetDescription()
    {
        return description;
    }
    public List<Hack> GetRemoveHacks()
    {
        return removeHacks;
    }

}
