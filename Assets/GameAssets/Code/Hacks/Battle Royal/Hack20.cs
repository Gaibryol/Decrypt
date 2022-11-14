using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack20 : Hack
{
    public Hack20()
    {
        description = "Decrypt the longest word";
    }

    public override void Initialize()
    {
        base.Initialize();
        gameController.DecryptLongestWord();
    }
}
