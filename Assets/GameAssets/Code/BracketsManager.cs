using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BracketsManager : MonoBehaviour
{
	public static BracketsManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}
}
