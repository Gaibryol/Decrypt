using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
	public List<Letter> Letters;

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
}
