using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Canvas Canvas;
	public Constants.GameStates GameState;

	public GameObject WordPrefab;

	private List<GameObject> lines;

	#region Game Variables
	private int abilityUsages = 1;
	private int maximumNumLines = 6;
	private float playerPoints = 0f;
	private int completedLines = 0;
	private float countDownTime = Constants.MaxTime;
	private float decryptTime = Constants.DecryptTime;

	private int warningLimit = Constants.WarningLimit;
	private float pointsMultiplier = 1f;
	#endregion
	

	[SerializeField] private float wordsYOffset;
	[SerializeField] private Vector3 bottomLinePos;
	[SerializeField] private Vector3 spawnPos;

	private void SpawnWord()
	{
		List<string> words = WordsManager.Instance.GetScrambledWord();

		GameObject newWord = Instantiate(WordPrefab, Canvas.transform);

		newWord.GetComponent<Word>().SpawnWord(words[0], words[1]);

		newWord.transform.localPosition = new Vector3(bottomLinePos.x, bottomLinePos.y + ((newWord.GetComponent<RectTransform>().rect.height + wordsYOffset) * lines.Count));

		lines.Add(newWord);

		UIManager.Instance.OnSpawnWord();

		if(lines.Count > maximumNumLines){
			//Game Over(To Restart Screen)
			Debug.Log("GameOver");
			GameState = Constants.GameStates.MainMenu;
		}
		else if(lines.Count >= maximumNumLines - warningLimit)
		{
			UIManager.Instance.DisplayWarning(true);
		}
	}

	public void CorrectWord(GameObject word)
	{
		int i = 0;
		i = lines.IndexOf(word);
		lines.Remove(word);
    
		// Need to bump all the other words down
		for(; i < lines.Count;i++)
		{
			
			lines[i].transform.localPosition = new Vector3(lines[i].transform.localPosition.x, lines[i].transform.localPosition.y - word.GetComponent<RectTransform>().rect.height - wordsYOffset);
		}

		if (lines.Count == 0)
		{
			UIManager.Instance.OnWordExit();
		}

		completedLines += 1;
		playerPoints += word.GetComponent<Word>().realWord.Length * Constants.PointsPerLetter * pointsMultiplier;
	
		UIManager.Instance.OnWordSolved(Mathf.FloorToInt(playerPoints));
		Destroy(word);

		if(lines.Count <maximumNumLines - warningLimit)
		{
			UIManager.Instance.DisplayWarning(false);
		}
	}

	public void SetMultiplier(float multiplier)
	{
		pointsMultiplier = multiplier;
	}
  
	public void DecryptWord(GameObject word)
	{
		if(abilityUsages > 0)
		{
			CorrectWord(word);
			abilityUsages -=1;
		}
	}
  
	public void DecryptList()
	{
		if(abilityUsages > 0)
		{
			for(int i = lines.Count-1; i >= 0 ;i--)
			{
				CorrectWord(lines[i]);
			}
      
			abilityUsages -=1;
		}
	}

	public void SetDecryptAmount(int amount)
	{
		abilityUsages = amount;
	}

	private GameObject GetLongestWord()
	{
		int maxLength = 0;
		GameObject longestWord = null;
		foreach(GameObject word in lines)
		{
			int wordLength = word.GetComponent<Word>().GetWordLength();
			if(wordLength > maxLength)
			{
				maxLength = wordLength;
				longestWord = word;
			}
		}
		return longestWord;
	}
  
	IEnumerator SpawnTwoWords()
    {
		  SpawnWord();
		  yield return new WaitForSeconds(0.5f);
		  SpawnWord();
    }
    
	public void ChangeMaxLife(int num)
	{
		maximumNumLines += num;
	}

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

		GameState = Constants.GameStates.MainMenu;
	}

	private void Start()
	{
		lines = new List<GameObject>();

		SpawnWord();
	}

	private void Update()
	{	
		decryptTime -= Time.deltaTime;
		countDownTime -= Time.deltaTime;
		if(countDownTime <= 0 || lines.Count == 0)
		{
			countDownTime = Constants.MaxTime;
			
			if(HacksManager.Instance.ActivatedH){
				StartCoroutine(SpawnTwoWords());
			}
			else
			{
				SpawnWord();
			}
		}
    
		if(decryptTime <= 0)
		{
			decryptTime = Constants.DecryptTime;
			if(HacksManager.Instance.ActivatedC)
			{
				GameObject longestWord = GetLongestWord();
				if(longestWord != null)
				{
					CorrectWord(longestWord);
				}
			}
			else if(HacksManager.Instance.ActivatedH)
			{
				if(lines.Count != 0)
				{
					GameObject randomWord = lines[UnityEngine.Random.Range(0,lines.Count)];
					CorrectWord(randomWord);
				}
			}
		}
	}
}
