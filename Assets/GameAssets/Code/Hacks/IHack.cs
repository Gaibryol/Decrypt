using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHack
{
    public string GetDescription();
    public List<Hack> GetRemoveHacks();
    public void Initialize(){}
    public void Apply(GameObject wordGameObject);
    public void Deactivate();

}
