using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChooseHack : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private CombinedHack combinedHack;
    private int stage;
    private GameController gameController;
    [SerializeField] private TMP_Text positiveText;
    [SerializeField] private TMP_Text negativeText;
    [SerializeField] private GameObject arrow;
    public void SetHack(CombinedHack h,GameController gC){
        combinedHack = h;
        gameController = gC;
        string[] decription = combinedHack.GetDescription().Split("|");
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
        HacksManager.Instance.AddCombinedHack(combinedHack);
        arrow.SetActive(false);
        this.gameObject.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        gameController.ChangeSubState(Constants.SubState.Playing);
    }
}