using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Hack : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private string hack;
    private int stage;
    private GameController gc;
    [SerializeField] private TMP_Text positiveText;
    [SerializeField] private TMP_Text negativeText;
    [SerializeField] private GameObject arrow;
    public void SetHack(string hackLetter, int stageLevel, GameController gameController){
        hack = hackLetter;
        stage = stageLevel;
        gc = gameController;
        string[] decription = HacksManager.Instance.GetDescription(hackLetter).Split("|");
        positiveText.text = decription[0];
        negativeText.text = decription[1];

    }
    public void OnPointerEnter(PointerEventData eventData)
	{
        arrow.SetActive(true);
        this.gameObject.transform.localScale = new Vector3(1,1,1);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        arrow.SetActive(false);
        this.gameObject.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
	}
    public void Clicked(){
        if(stage != 3)
        {
            HacksManager.Instance.AddEarlyHack(hack);
        }
        else
        {
            HacksManager.Instance.AddLateHack(hack);
        }
        gc.ChangeSubState(Constants.SubState.Playing);
    }
}
