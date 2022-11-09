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
	public List<GameObject> letters;
	
	private HorizontalLayoutGroup horizontalLayoutGroup;

	private GameController gameController;

	public void SpawnWord(GameController controller, string word, string scrambled, int currentStage, bool alternateColor)
	{
		gameController = controller;
		realWord = word;
		scrambledWord = scrambled;
		IsMoving = true;
		IsInteractable = false;

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
			letters.Add(newObj);
			Letter lScript = newObj.GetComponent<Letter>();
			lScript.Construct(scrambledWord[i].ToString(),color);


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

	public int GetIndexByPos(float x)
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			if (x  < transform.GetChild(i).localPosition.x)
			{
				return i;
			}
		}

		return transform.childCount;
	}

	public void CheckIsCorrect()
	{
		string letters = "";

		foreach(Transform obj in transform)
		{
			letters += obj.GetComponent<Letter>().Character;
		}

		if (WordsList.Instance.IfContains(letters) || letters == realWord)
		{
			gameController.CorrectWord(this.gameObject);
		}
	}

	public void OnPickUpLetter()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("PickupLetter");
        //horizontalLayoutGroup.enabled = false;
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
			if(Constants.Hack4.activated & IsInteractable)
			{
				Constants.Hack4.RightClick(this.gameObject);
			}
		}
	}

	public void SolveWord(){
		int i = 0;
		foreach (Transform obj in transform)
		{
			obj.GetComponent<Letter>().ReplaceLetter(realWord[i].ToString());
			i++;
		}
	}
	public void ShowIsCorrect()
	{
		IsInteractable = false;
		foreach (Transform obj in transform)
		{
			obj.GetComponent<Letter>().ChangeToCorrect();
		}
	}
}
