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
	
	private HorizontalLayoutGroup horizontalLayoutGroup;

	public void SpawnWord(string word, string scrambled)
	{
		realWord = word;
		scrambledWord = scrambled;
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

		if (letters == realWord)
		{
			GameManager.Instance.CorrectWord(this.gameObject);
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
		UIManager.Instance.OnWordHover(transform.localPosition.y);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.Instance.OnWordExit();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right){
			if(HacksManager.Instance.ActivatedE)
			{
				GameManager.Instance.DecryptWord(this.gameObject);
			}
			else if(HacksManager.Instance.ActivatedG)
			{
				//Drop Instantly
			}
			else if(HacksManager.Instance.ActivatedI){
				GameManager.Instance.DecryptList();
			}
		}
	}

	public int GetWordLength()
	{
		return realWord.Length;
	}
}
