using System.Collections.Generic;
using UnityEngine;
//"Right-click to decrypt a word instantly (Up to 4 uses in a game)|Letters only appear when hovering over the letter tile";
public class Hack4 : Hack
{
    public Hack4()
    {
        description = "Right-click to decrypt a word instantly (Up to 4 uses in a game)|Letters only appear when hovering over the letter tile";
        removeHacks = new List<Hack>{Constants.Hack2,Constants.Hack5,Constants.Hack6,Constants.Hack8};
    }
    public override void Initialize()
    {
        gameUIController.DisplayAbility("RIGHT CLICK TO DESTROY A WORD", 4);
        gameController.SetDecryptAmount(4);
        base.Initialize();
    }

    public override void Apply(GameObject wordGameObject){
        Word word = wordGameObject.GetComponent<Word>();
		foreach (Transform obj in wordGameObject.transform)
		{
            obj.GetComponent<Letter>().IsHover();
		}
    }
    public void RightClick(GameObject wordGameObject)
    {
        gameController.DecryptWord(wordGameObject);
    }
}
