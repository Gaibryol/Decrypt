using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using ExitGames.Client.Photon;

public class GameController : MonoBehaviourPunCallbacks, IPointerClickHandler
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
    private float gameTime;

    private int seed;

	private int warningLimit;
	private float pointsMultiplier;

	private Constants.SubState subState;
    private Constants.GameType gameType;

	private int currentStage;
	private bool alternateColor;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            InitPhotonRoomPrefs();
        } else
        {
            StartGame();
        }
    }
	private void Update()
	{
		if(subState != Constants.SubState.Playing) return;
		
		countDownTime -= Time.deltaTime;
        decryptTime -= Time.deltaTime;
        gameTime += Time.deltaTime;

        if (countDownTime <= 0 || lines.Count == 0)
		{
			countDownTime = Constants.MaxTime;
			SpawnWord();
			gameUI.ResetTimer();
		}

        if (gameType == Constants.GameType.Timed && gameTime >= 30f)
        {
            StopAllCoroutines();
            gameUI.CompleteGame();
        }
    }

    public void StartGame()
	{
		InitVariables();
		gameUI.StartGame();
	}
    
    private void InitPhotonRoomPrefs()
    {
        object val = GetRoomCustomProperty("GameType");
        if (val == null) return;
        gameType = (Constants.GameType)val;


        val = GetRoomCustomProperty("Seed");
        if (val == null) return;
        seed = (int) val;
        WordsManager.Instance.SetSeed(seed);

        StartGame();

    }

    private object GetRoomCustomProperty(string key)
    {
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
        object temp;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out temp))
        {
            return temp;
        }
        return null;
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
        gameTime = 0f;

		currentStage = 1;
		alternateColor = false;

		warningLimit = Constants.WarningLimit;
		pointsMultiplier = 1f;

		ChangeSubState(Constants.SubState.Playing);

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
		HacksManager.Instance.Apply(newWord);
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
	}
	private IEnumerator CorrectWordAnim(GameObject word)
	{
		int i = lines.IndexOf(word);
		word.GetComponent<Word>().ShowIsCorrect();
		// wait for 0.75 second
		yield return new WaitForSeconds(0.75f);
		playerPoints += word.GetComponent<Word>().realWord.Length * Constants.PointsPerLetter * pointsMultiplier;
		gameUI.OnWordExit();
		gameUI.OnWordSolved(Mathf.FloorToInt(playerPoints));
		GameObject tempLetter = GameObject.Find("PickedUp");
		if(tempLetter != null )
		{
			if(word.GetComponent<Word>().letters.Contains(tempLetter)){
				tempLetter.GetComponent<Letter>().ChangeToCorrect();
				Destroy(tempLetter);
			}
		}
		lines.Remove(word);
		Destroy(word);

		// Need to bump all the other words down
		for (; i < lines.Count; i++)
		{
			if (!lines[i].GetComponent<Word>().IsMoving)
			{
				lines[i].transform.localPosition = new Vector3(lines[i].transform.localPosition.x, lines[i].transform.localPosition.y - word.GetComponent<RectTransform>().rect.height - WordsYOffset);
			}
		}

		if (lines.Count < MaximumNumLines - warningLimit)
		{
			gameUI.DisplayWarning(false);
		}
		if (playerPoints >= 10000 & currentStage == 1)
		{
			WordsManager.Instance.ChangeWordLengths(new List<int>(){3,4,5});
			SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
			ChangeSubState(Constants.SubState.Hack);
			currentStage += 1;
			gameUI.OnStageComplete(currentStage);
		}
		else if (playerPoints >= 25000 & currentStage == 2)
		{
			WordsManager.Instance.ChangeWordLengths(new List<int>(){4,5,6});
			SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
			ChangeSubState(Constants.SubState.Hack);
			currentStage += 1;
			gameUI.OnStageComplete(currentStage);
		}
		// else if (playerPoints >= 50000 & currentStage == 3)
		// {
		// 	WordsManager.Instance.ChangeWordLengths(new List<int>(){5,6,7});
		// 	SoundEffectsManager.Instance.PlayOneShotSFX("StageEnded");
		// 	currentStage += 1;
		// 	gameUI.OnStageComplete(currentStage);
		// }
	}

	public void NewGame()
	{
		ResetList();
		InitVariables();
		gameUI.StartGame();
		HacksManager.Instance.InitVariables();
		WordsManager.Instance.InitVariables();
	}

	public IEnumerator MoveWord(GameObject newWord)
	{
		Vector3 newPos = new Vector3();

		while (newWord != null && newWord.transform.localPosition != newPos)
		{
			if(subState != Constants.SubState.Pause)
			{
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
					newWord.transform.localPosition = Vector3.MoveTowards(newWord.transform.localPosition, newPos, 400f * Time.deltaTime);
			}
			yield return null;
		}
		if(newWord != null){
			foreach(Transform child in newWord.transform)
			{
				child.GetComponent<Letter>().RevealLetter();
			}
			newWord.GetComponent<Word>().IsMoving = false;
			newWord.GetComponent<Word>().IsInteractable = true;
		}
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
				List<Hack> hacks = HacksManager.Instance.GenerateHacks();
				Hack1.GetComponent<ChooseHack>().SetHack(hacks[0],this);
				Hack2.GetComponent<ChooseHack>().SetHack(hacks[1],this);
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
			word.GetComponent<Word>().SolveWord();
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
				lines[i].GetComponent<Word>();
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

	public void DecryptLongestWord()
	{
		int maxLength = 0;
		GameObject longestWord = null;
		foreach (GameObject word in lines)
		{
			int wordLength = word.GetComponent<Word>().realWord.Length;
			if (wordLength > maxLength)
			{
				maxLength = wordLength;
				longestWord = word;
			}
		}
		longestWord.GetComponent<Word>().SolveWord();
		CorrectWord(longestWord);
	}

	public void TeleportWord(){
		GameObject wordGameObject = lines[lines.Count-1];
		wordGameObject.transform.localPosition = new Vector3(0, bottomLineY + ((wordGameObject.GetComponent<RectTransform>().rect.height + WordsYOffset) * lines.Count) + ((wordGameObject.GetComponent<RectTransform>().rect.height + WordsYOffset) * Mathf.Abs(MaximumNumLines - defaultMaxLines)));
	}

	public void DecryptRandomWord(){
		GameObject randomWord = lines[Random.Range(0, lines.Count)];
		randomWord.GetComponent<Word>().SolveWord();
		CorrectWord(randomWord);
	}
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        InitPhotonRoomPrefs();
    }
    public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			if(Constants.Hack6.activated)
			{
				Constants.Hack6.RightClick();
			}
			else if(Constants.Hack8.activated){
				Constants.Hack8.RightClick();
			}
		}

	}
}
