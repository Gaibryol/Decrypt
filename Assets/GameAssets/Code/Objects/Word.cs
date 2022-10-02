using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Word : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] public GameObject letterPrefab;

	public string realWord;
	public string scrambledWord;

	private HorizontalLayoutGroup horizontalLayoutGroup;

	public void SpawnWord(string word, string scrambled)
	{
		realWord = word;
		scrambledWord = scrambled;

		for(int i = 0; i < scrambledWord.Length; i++)
		{
			GameObject newObj = Instantiate(letterPrefab, transform);
			Letter lScript = newObj.GetComponent<Letter>();

			lScript.Construct(scrambledWord[i].ToString(), false, true);
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
}
