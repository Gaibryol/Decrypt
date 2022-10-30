using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//"Decrypt the longest words every 25 seconds|Each word has a chance of having a letter obscured";
public class Hack2 : Hack
{
    private float decryptTime;
    public Hack2()
    {
        description = "Decrypt the longest words every 25 seconds|Each word has a chance of having a letter obscured";
        removeHacks = new List<Hack>{Constants.Hack4,Constants.Hack7};
        decryptTime = Constants.DecryptTime;
    }

    public override void Apply(GameObject wordGameObject)
    {
        Word word = wordGameObject.GetComponent<Word>();
        if(Random.Range(0,3) == 3){
            word.letters[Random.Range(0,word.letters.Count)].GetComponent<Letter>().NeverReveal();
        }
        
    }
    public void Update(){
        if(activated){
            Debug.Log(decryptTime);
            decryptTime -= Time.deltaTime;
            if (decryptTime <= 0)
            {
                gameController.DecryptLongestWord();
		        decryptTime = Constants.DecryptTime;
            }
        }
    }
}
