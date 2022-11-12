using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack5 : Hack
{
    public Hack5()
    {
        description = "Letters can be seen as they’re falling down|See 1 less row";
        removeHacks = new List<Hack>{HM.Hack0, HM.Hack4};
    }
    public override void Initialize()
    {
        gameController.ChangeMaxLife(-1);
        base.Initialize();
    }
    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        foreach (Transform obj in wordGameObject.transform)
		{
			obj.GetComponent<Letter>().RevealLetter();
		}
    }
}
