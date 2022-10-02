using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

	[SerializeField, Header("Objects")] private GameObject brackets;
	[SerializeField] private GameObject topBanner;
	[SerializeField] private GameObject indicator;

	[SerializeField, Header("Text")] private TMP_Text score;
	[SerializeField] private TMP_Text stage;

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

		brackets.SetActive(false);
		indicator.SetActive(false);

		score.text = "0";
		stage.text = "01";
	}

	public void OnWordHover(float y)
	{
		brackets.SetActive(true);
		brackets.transform.localPosition = new Vector3(brackets.transform.localPosition.x, y);

		indicator.SetActive(true);
		indicator.transform.localPosition = new Vector3(indicator.transform.localPosition.x, y);
	}

	public void OnWordExit()
	{
		brackets.SetActive(false);
		indicator.SetActive(false);
	}

	public void OnSpawnWord()
	{
		topBanner.transform.SetAsLastSibling();
	}

	public void OnStageComplete(int newStage)
	{
		stage.text = "0" + newStage.ToString();
	}

	public void OnWordSolved(int amount)
	{
		int newScore = int.Parse(score.text) + amount;
		score.text = newScore.ToString();
	}
}
