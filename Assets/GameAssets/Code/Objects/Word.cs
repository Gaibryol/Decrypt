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

	public void SpawnWord(GameController controller, string word, string scrambled, int currentStage, bool alternateColor)
	{
		gameController = controller;
		realWord = word;
		scrambledWord = scrambled;
		IsMoving = true;
		IsInteractable = false;
		int randomNumber = Random.Range(0,realWord.Length);

		Constants.LetterColors color = Constants.LetterColors.Blue;
		if (currentStage == 1 && alternateColor)
		{
			color = Constants.LetterColors.Blue;
		}
		else if (currentStage == 1 && !alternateColor)
		{
			color = Constants.LetterColors.Orange;
		}
		else if (currentStage == 2 && alternateColor)
		{
			color = Constants.LetterColors.Red;
		}
		else if (currentStage == 2 && !alternateColor)
		{
			color = Constants.LetterColors.Yellow;
		}
		else if (currentStage == 3 && alternateColor)
		{
			color = Constants.LetterColors.Pink;
		}
		else if (currentStage == 3 && !alternateColor)
		{
			color = Constants.LetterColors.Purple;
		}

		for (int i = 0; i < scrambledWord.Length; i++)
		{
			GameObject newObj = Instantiate(letterPrefab, transform);
			Letter lScript = newObj.GetComponent<Letter>();
			bool covered = true;
			if(HacksManager.Instance.ActivatedF){
				covered = false;
			}
			if(HacksManager.Instance.ActivatedA & i == 0){
				lScript.Construct(scrambledWord[i].ToString(), covered, false, color,false);
			}
			else if(HacksManager.Instance.ActivatedB & i == scrambledWord.Length -1)
			{
				lScript.Construct(scrambledWord[i].ToString(), covered, false, color,false);
			}
			else if(HacksManager.Instance.ActivatedC & i == randomNumber)
			{
				lScript.Construct(scrambledWord[i].ToString(), true, true, color,true);
			}
			else if(HacksManager.Instance.ActivatedE)
			{
				lScript.Construct(scrambledWord[i].ToString(), covered, true, color,true);
			}
			else
			{
				lScript.Construct(scrambledWord[i].ToString(), covered, true, color,false);
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
		SoundEffectsManager.Instance.PlayOneShotSFX("PickupLetter");
		horizontalLayoutGroup.enabled = false;
	}

	public void OnPutDownLetter()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("PutdownLetter");
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
			if(HacksManager.Instance.ActivatedE & IsInteractable)
			{
				gameController.DecryptWord(this.gameObject);
			}
		}
	}

	public int GetWordLength()
	{
		return realWord.Length;
	}
}
