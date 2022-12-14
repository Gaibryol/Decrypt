using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack8 : Hack
{
    public Hack8()
    {
        description = "Decrypt all words (One use per game)|See 2 less lines";
        removeHacks = new List<Hack>{HM.Hack4, HM.Hack6};
    }
    public override void Initialize()
    {
        gameController.ChangeMaxLife(-2);
        gameUIController.DisplayAbility("RIGHT CLICK TO CLEAR BOARD",1);
        gameController.SetDecryptAmount(1);
        base.Initialize();
    }

    public override void Apply(GameObject wordGameObject)
    {
        
    }
    public void RightClick()
    {
        gameController.DecryptList();
    }
}

