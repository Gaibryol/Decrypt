using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack30 : Hack
{
    public Hack30()
    {
        description = "Add a line to all other players";
    }

    public override void Initialize()
    {
        base.Initialize();
        gameController.SpawnWord();
        
    }
}
