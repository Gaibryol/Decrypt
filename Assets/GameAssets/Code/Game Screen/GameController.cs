using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] public GameObject WordPrefab;
	[SerializeField] public Canvas Canvas;
	[SerializeField] private GameObject Hack1;
	[SerializeField] private GameObject Hack2;

	[SerializeField] public float WordsYOffset;
	[SerializeField] private float bottomLineY;
	[SerializeField] private float spawnY;

	private GameUIController gameUI;

	private List<GameObject> lines;

	private int abilityUsages;
	public int MaximumNumLines;
	private int defaultMaxLines;
	private float playerPoints;
	private float countDownTime;
	private float decryptTime;

	private int warningLimit;
	private float pointsMultiplier;

	private Constants.SubState subState;

	private int currentStage;
	private bool alternateColor;

	public void StartGame()
	{
		InitVariables();
		gameUI.StartGame();
	}

	private void InitVariables()
	{
		lines = new List<GameObject>();

		abilityUsages = 0;
		MaximumNumLines = 6;
		defaultMaxLines = 6;
		playerPoints = 0f;
		countDownTime = Constants.MaxTime;
		decryptTime = Constants.DecryptTime;

		currentStage = 1;
		alternateColor = false;

		warningLimit = Constants.WarningLimit;
		pointsMultiplier = 1f;
		subState = Constants.SubState.Playing;

		gameUI = GetComponent<GameUIController>();
	}

	private void SpawnWord()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("WordSpawned");

		List<string> words = WordsManager.Instance.GetScrambledWord();

		GameObject newWord = Instantiate(WordPrefab, Canvas.transform);
		newWord.transform.SetParent(Canvas.transform.Find("GameScreen").transform);
		newWord.transform.localPosition = new Vector3(newWord.transform.localPosition.x, spawnY);

		newWord.GetComponent<Word>().SpawnWord(this, words[0], words[1], currentStage, alternateColor);

		alternateColor = !alternateColor;

		lines.Add(newWord);
		StartCoroutine(MoveWord(newWord));

		gameUI.OnSpawnWord();

		if (lines.Count > MaximumNumLines)
		{
			StopAllCoroutines();
			gameUI.CompleteGame();
		}
		else if (lines.Count > MaximumNumLines - warningLimit)
		{
			SoundEffectsManager.Instance.PlayOneShotSFX("Warning");
			gameUI.DisplayWarning(true);
		}
	}

	private void ResetList()
	{
		StopAllCoroutines();
		if (lines != null)
		{
			for(int i = lines.Count-1; i>=0; i--){
				GameObject tempObj = lines[i];
				lines.Remove(lines[i]);
				Destroy(tempObj);
			}
		}
		countDownTime = Constants.MaxTime;
		decryptTime = Constants.DecryptTime;
	}

	public void NewGame()
	{
		ResetList();
		InitVariables();
		gameUI.InitVariables();
		HacksManager.Instance.InitVariables();
		WordsManager.Instance.InitVariables();
	}

	public IEnumerator MoveWord(GameObject newWord)
	{
		Vector3 newPos = new Vector3();
		if (MaximumNumLines > defaultMaxLines)
		{
			newPos = new Vector3(0, bottomLineY + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * lines.Count) - ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * (MaximumNumLines - defaultMaxLines)));
		}
		else if (MaximumNumLines < defaultMaxLines)
		{
			newPos = new Vector3(0, bottomLineY + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * lines.Count) + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * (defaultMaxLines - MaximumNumLines)));
		}
		else
		{
			newPos = new Vector3(0, bottomLineY + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * lines.Count));
		}
		
		int currentNumWords = lines.Count;

		while (newWord.transform.localPosition != newPos)
		{
			if(subState != Constants.SubState.Pause)
			{
				if (lines.Count != currentNumWords)
				{
					newPos = new Vector3(newPos.x, newPos.y - (newWord.GetComponent<RectTransform>().rect.height + WordsYOffset));
					currentNumWords = lines.Count;
				}

				newWord.transform.localPosition = Vector3.MoveTowards(newWord.transform.localPosition, newPos, 250f * Time.deltaTime);
			}
			yield return null;
		}
		foreach(Transform child in newWord.transform)
		{
			child.GetComponent<Letter>().RevealLetter();
		}
		newWord.GetComponent<Word>().IsMoving = false;
		newWord.GetComponent<Word>().IsInteractable = true;
	}

	public void ChangeSubState(Constants.SubState state){
		subState = state;
		switch(state)
		{
			case(Constants.SubState.Playing):
				SoundEffectsManager.Instance.PlayGameMusic();
				for (int i = 0; i <lines.Count;i++)
				{
					if (lines[i].GetComponent<Word>().IsMoving!= true)
					{
						lines[i].GetComponent<Word>().IsInteractable = true;
					}
				}
				break;
			case(Constants.SubState.Pause):
				for(int i = 0; i<lines.Count;i++)
				{
					lines[i].GetComponent<Word>().IsInteractable = false;
				}
				break;
			case(Constants.SubState.Help):
				break;			
			case(Constants.SubState.Complete):
				SoundEffectsManager.Instance.PlayEverythingElseMusic();
				break;
			case(Constants.SubState.Hack):
				SoundEffectsManager.Instance.PlayEverythingElseMusic();
				ResetList();
				List<string> hacks = HacksManager.Instance.GenerateHacks(currentStage);
				Hack1.GetComponent<Hack>().SetHack(hacks[0],currentStage,this);
				Hack2.GetComponent<Hack>().SetHack(hacks[1],currentStage,this);
				gameUI.DisplayHacks();
				break;
		}
	}
	
	public void OnWordHover(float y)
	{
		gameUI.OnWordHover(y);
	}

	public void OnWordExit()
	{
		gameUI.OnWordExit();
	}

	private IEnumerator CorrectWordAnim(GameObject word)
	{
		int i = 0;
		i = lines.IndexOf(word);
		lines.Remove(word);

		word.GetComponent<Word>().ShowIsCorrect();

		// wait for 1 second
		yield return new WaitForSeconds(1f);

		// Need to bump all the other words down
		for (; i < lines.Count; i++)
		{
			if (!lines[i].GetComponent<Word>().IsMoving)
			{
				lines[i].transform.localPosition = new Vector3(lines[i].transform.localPosition.x, lines[i].transform.localPosition.y - word.GetComponent<RectTransform>().rect.height - WordsYOffset);
			}
		}

		playerPoints += word.GetComponent<Word>().realWord.Length * Constants.PointsPerLetter * pointsMultiplier;

		gameUI.OnWordExit();

		gameUI.OnWordSolved(Mathf.FloorToInt(playerPoints));
		Destroy(word);

		if (lines.Count < MaximumNumLines - warningLimit)
		{
			gameUI.DisplayWarning(false);
		}
		if (playerPoints >= 10000 & currentStage == 1)
		{
			WordsManager.Instance.ChangePossibleWordLength("4,5,6");
			SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
			ChangeSubState(Constants.SubState.Hack);
			currentStage += 1;
			gameUI.OnStageComplete(currentStage);
		}
		else if (playerPoints >= 25000 & currentStage == 2)
		{
			WordsManager.Instance.ChangePossibleWordLength("5,6,7");
			SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
			ChangeSubState(Constants.SubState.Hack);
			currentStage += 1;
			gameUI.OnStageComplete(currentStage);
		}
		else if (playerPoints >= 50000 & currentStage == 3)
		{
			SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
			ChangeSubState(Constants.SubState.Hack);
			currentStage += 1;
			gameUI.OnStageComplete(currentStage);
		}
	}

	public void CorrectWord(GameObject word)
	{
		StartCoroutine(CorrectWordAnim(word));
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
			gameUI.LowerDiplayAbility();
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
			gameUI.LowerDiplayAbility();
		}
	}

	public void SetDecryptAmount(int amount)
	{
		abilityUsages = amount;
	}

	public void ChangeMaxLife(int num)
	{
		MaximumNumLines += num;
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

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			if(HacksManager.Instance.ActivatedG)
			{
				GameObject newWord = lines[lines.Count-1];
				newWord.transform.localPosition = new Vector3(0, bottomLineY + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * lines.Count) + ((newWord.GetComponent<RectTransform>().rect.height + WordsYOffset) * Mathf.Abs(MaximumNumLines - defaultMaxLines)));
			}
			else if(HacksManager.Instance.ActivatedI){
				DecryptList();
			}
		}
	}

	public IEnumerator SpawnTwoWords()
	{
		SpawnWord();
		yield return new WaitForSeconds(1.5f);
		SpawnWord();
	}

	private void Update()
	{
		if(subState != Constants.SubState.Playing) return;
		
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
			gameUI.ResetTimer();
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
