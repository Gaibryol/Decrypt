using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack7 : Hack
{
    private float decryptTime;
    public Hack7()
    {
        description = "Decrypt a random word every 25 seconds|See 3 less row";
        removeHacks = new List<Hack>{ HM.Hack3};
        decryptTime = Constants.DecryptTime;
    }
    public override void Initialize()
    {
        gameController.ChangeMaxLife(-3);
        base.Initialize();
    }
    public void Update() {
        if(activated){
            decryptTime -= Time.deltaTime;
            if (decryptTime <= 0)
            {
                gameController.DecryptRandomWord();
		        decryptTime = Constants.DecryptTime;
            }
        }
    }
}
