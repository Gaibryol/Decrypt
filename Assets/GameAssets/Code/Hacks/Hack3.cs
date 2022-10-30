using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//"Increase the maximum amount of rows by 2|Only the bottom three rows can be seen";
public class Hack3 : Hack
{
    public Hack3()
    {
        description = "Increase the maximum amount of rows by 2|Only the bottom three rows can be seen";
        removeHacks = new List<Hack>{};
    }
    public override void Initialize()
    {
        HacksManager.Instance.ShowAmount = 3;
        gameController.ChangeMaxLife(2);
        base.Initialize();
    }
}
