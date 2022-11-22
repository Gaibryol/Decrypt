using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedHack 
{
    private string description;
    private Hack positiveHack;
    private Hack negativeHack;
    public CombinedHack(Hack ph, Hack nh)
    {
        description = ph.GetDescription() + "|" + nh.GetDescription();
        positiveHack = ph;
        negativeHack = nh;
    }
    public string GetDescription(){
        return description;
    }
    public Hack GetPositiveHack(){
        return positiveHack;
    }
    public Hack GetNegativeHack(){
        return negativeHack;
    }
}
