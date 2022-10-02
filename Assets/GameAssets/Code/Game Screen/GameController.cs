using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] public GameObject WordPrefab;
	[SerializeField] public Canvas Canvas;

	[SerializeField] private float wordsYOffset;
	[SerializeField] private Vector3 bottomLinePos;
	[SerializeField] private Vector3 spawnPos;

	private GameUIController gameUI;

	private List<GameObject> lines;

	private int abilityUsages;
	private int maximumNumLines;
	private float playerPoints;
	private int completedLines;
	private float countDownTime;
	private float decryptTime;

	private int warningLimit;
	private float pointsMultiplier;

	private int currentStage;

	public void StartGame()
	{
		InitVariables();

		// Start game
		SpawnWord();
	}

	private void InitVariables()
	{
		lines = new List<GameObject>();

		abilityUsages = 1;
		maximumNumLines = 6;
		playerPoints = 0f;
		completedLines = 0;
		countDownTime = Constants.MaxTime;
		decryptTime = Constants.DecryptTime;

		currentStage = 0;

		warningLimit = Constants.WarningLimit;
		pointsMultiplier = 1f;

		gameUI = GetComponent<GameUIController>();
	}

	private void SpawnWord()
	{
		List<string> words = WordsManager.Instance.GetScrambledWord();

		GameObject newWord = Instantiate(WordPrefab, Canvas.transform);
		newWord.transform.SetParent(Canvas.transform.Find("GameScreen").transform);
		newWord.transform.localPosition = spawnPos;

		newWord.GetComponent<Word>().SpawnWord(this, words[0], words[1]);
	
		// newWord.transform.localPosition = new Vector3(bottomLinePos.x, bottomLinePos.y + ((newWord.GetComponent<RectTransform>().rect.height + wordsYOffset) * lines.Count));

		StartCoroutine(MoveWord(newWord));
		lines.Add(newWord);

		gameUI.OnSpawnWord();

		if (lines.Count > maximumNumLines)
		{
			//Game Over(To Restart Screen)
			Debug.Log("GameOver");
		}
		else if (lines.Count >= maximumNumLines - warningLimit)
		{
			gameUI.DisplayWarning(true);
		}
	}

	public IEnumerator MoveWord(GameObject newWord){

		Vector3 newPos = new Vector3(bottomLinePos.x, bottomLinePos.y + ((newWord.GetComponent<RectTransform>().rect.height + wordsYOffset) * lines.Count));
		while(newWord.transform.localPosition != newPos){
			newWord.transform.localPosition = Vector3.MoveTowards(newWord.transform.localPosition, newPos, 0.5f);
			yield return null;
		}
		newWord.GetComponent<Word>().IsMoving = false;
	}
	
	public void OnWordHover(float y)
	{
		gameUI.OnWordHover(y);
	}

	public void OnWordExit()
	{
		gameUI.OnWordExit();
	}

	public void CorrectWord(GameObject word)
	{
		int i = 0;
		i = lines.IndexOf(word);
		lines.Remove(word);

		// Need to bump all the other words down
		for (; i < lines.Count; i++)
		{
			lines[i].transform.localPosition = new Vector3(lines[i].transform.localPosition.x, lines[i].transform.localPosition.y - word.GetComponent<RectTransform>().rect.height - wordsYOffset);
		}

		completedLines += 1;
		playerPoints += word.GetComponent<Word>().realWord.Length * Constants.PointsPerLetter * pointsMultiplier;

		gameUI.OnWordExit();
		gameUI.OnWordSolved(1000);

		gameUI.OnWordSolved(Mathf.FloorToInt(playerPoints));
		Destroy(word);

		if (lines.Count < maximumNumLines - warningLimit)
		{
			gameUI.DisplayWarning(false);
		}
	}

	public void SetMultiplier(float multiplier)
	{
		pointsMultiplier = multiplier;
	}

	public void DecryptWord(GameObject word)
	{
		if (abilityUsages > 0)
		{
			CorrectWord(word);
			abilityUsages -= 1;
		}
	}

	public void DecryptList()
	{
		if (abilityUsages > 0)
		{
			for (int i = lines.Count - 1; i >= 0; i--)
			{
				CorrectWord(lines[i]);
			}

			abilityUsages -= 1;
		}
	}

	public void SetDecryptAmount(int amount)
	{
		abilityUsages = amount;
	}

	public void ChangeMaxLife(int num)
	{
		maximumNumLines += num;
	}

	private GameObject GetLongestWord()
	{
		int maxLength = 0;
		GameObject longestWord = null;
		foreach (GameObject word in lines)
		{
			int wordLength = word.GetComponent<Word>().GetWordLength();
			if (wordLength > maxLength)
			{
				maxLength = wordLength;
				longestWord = word;
			}
		}
		return longestWord;
	}

	public IEnumerator SpawnTwoWords()
	{
		SpawnWord();
		yield return new WaitForSeconds(0.5f);
		SpawnWord();
	}

	private void Update()
	{
		decryptTime -= Time.deltaTime;
		countDownTime -= Time.deltaTime;
		if (countDownTime <= 0 || lines.Count == 0)
		{
			countDownTime = Constants.MaxTime;

			if (HacksManager.Instance.ActivatedH)
			{
				StartCoroutine(SpawnTwoWords());
			}
			else
			{
				SpawnWord();
			}
		}

		if (decryptTime <= 0)
		{
			decryptTime = Constants.DecryptTime;
			if (HacksManager.Instance.ActivatedC)
			{
				GameObject longestWord = GetLongestWord();
				if (longestWord != null)
				{
					CorrectWord(longestWord);
				}
			}
			else if (HacksManager.Instance.ActivatedH)
			{
				if (lines.Count != 0)
				{
					GameObject randomWord = lines[Random.Range(0, lines.Count)];
					CorrectWord(randomWord);
				}
			}
		}
	}
}
