using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Word : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] public GameObject letterPrefab;

	public string realWord;
	public string scrambledWord;
	public bool IsInteractable;
	public bool IsMoving;
	
	private HorizontalLayoutGroup horizontalLayoutGroup;

	private GameController gameController;


	public void SpawnWord(GameController controller, string word, string scrambled)
	{
		gameController = controller;
		realWord = word;
		scrambledWord = scrambled;
		IsMoving = true;
		IsInteractable = false;
		int randomNumber = Random.Range(0,realWord.Length);

		for(int i = 0; i < scrambledWord.Length; i++)
		{
			GameObject newObj = Instantiate(letterPrefab, transform);
			Letter lScript = newObj.GetComponent<Letter>();
			if(HacksManager.Instance.ActivatedC & i == randomNumber)
			{
				lScript.Construct(scrambledWord[i].ToString(), true, true);
			}
			else if(HacksManager.Instance.ActivatedE)
			{
				lScript.Construct(scrambledWord[i].ToString(), true, true);
			}
			else
			{
				lScript.Construct(scrambledWord[i].ToString(), false, true);
			}
		}
		horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
	}

	public int GetIndex(GameObject letter)
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject == letter)
			{
				return i;
			}
		}

		return -1;
	}

	public void CheckIsCorrect()
	{
		string letters = "";

		foreach(Transform obj in transform)
		{
			letters += obj.GetComponent<Letter>().Character;
		}

		if (Contains.Instance.IfContains(letters) || letters == realWord)
		{
			gameController.CorrectWord(this.gameObject);
		}
	}

	public void OnPickUpLetter()
	{
		horizontalLayoutGroup.enabled = false;
	}

	public void OnPutDownLetter()
	{
		horizontalLayoutGroup.enabled = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(!IsInteractable) return;

		gameController.OnWordHover(transform.localPosition.y);

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(!IsInteractable) return;

		gameController.OnWordExit();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			if(HacksManager.Instance.ActivatedE)
			{
				gameController.DecryptWord(this.gameObject);
			}
			else if(HacksManager.Instance.ActivatedG)
			{
				//Drop Instantly
			}
			else if(HacksManager.Instance.ActivatedI){
				gameController.DecryptList();
			}
		}
	}

	public int GetWordLength()
	{
		return realWord.Length;
	}
}
