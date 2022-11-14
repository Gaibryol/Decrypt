using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameUIController : MonoBehaviourPunCallbacks
{
	[SerializeField, Header("Objects")] private GameObject brackets;
	[SerializeField] private GameObject topBanner;
	[SerializeField] private GameObject bracketAnim;
	[SerializeField] private GameObject indicator;
	[SerializeField] private GameObject cover;
	[SerializeField] private GameObject warning;
	[SerializeField] private GameObject pause;
	[SerializeField] private GameObject hack;
	[SerializeField] private GameObject complete;
    [SerializeField] private GameObject room;
	[SerializeField] private GameObject abilityPrefab;
	[SerializeField] private GameObject abilityParent;
	[SerializeField] private List<GameObject> displayHacks;
	[SerializeField] public Image baseLine;
	[SerializeField] private GameObject tutorial;
    [SerializeField] public GameObject maxLine;

	[SerializeField] private float startingBaselineY;

	[SerializeField, Header("Text")] private TMP_Text score;
	[SerializeField] private TMP_Text stage;
	[SerializeField] private TMP_Text ability;
	[SerializeField] private TMP_Text pointsCompleted;
	[SerializeField] private TMP_Text stageCompleted;

	[SerializeField,Header("Sprite")] private Sprite hackCompleted;
	

	[SerializeField, Header("Indicator Animations")] private Animator indicatorAnim;

	[SerializeField, Header("Buttons")] private Toggle helpButton;
	[SerializeField] private Toggle pauseButton;
	[SerializeField] private Animator timer;
	[SerializeField] private Toggle secondHelpToggle;

    [Header("Battle Royal")]
    [SerializeField] private Button negativeButton;
    [SerializeField] private Button positiveButton;

	private GameController gameController;
	private List<GameObject> abilityUses;

    public void StartGame()
	{
		InitVariables();
		DisplayBaseline();
	}

	public void InitVariables()
	{
		brackets.SetActive(false);
		indicator.SetActive(false);
		cover.SetActive(false);

		foreach(Transform child in abilityParent.transform)
		{
			Destroy(child.gameObject);
		}

		abilityUses = new List<GameObject>();
		score.text = "0";
		stage.text = "01";
		ability.text = "None";
		gameController = GetComponent<GameController>();
		warning.SetActive(false);
	}

	public void OnWordHover(float y)
	{
		brackets.SetActive(true);
		brackets.transform.localPosition = new Vector3(brackets.transform.localPosition.x, y);
		bracketAnim.transform.localPosition = brackets.transform.localPosition;

		indicator.SetActive(true);
		indicator.transform.localPosition = new Vector3(indicator.transform.localPosition.x, y);

		indicatorAnim.Play("Solving");
	}

	public void DisplayWarning(bool show)
	{
		warning.SetActive(show);
	}

	public void OnWordExit()
	{
		brackets.SetActive(false);
		indicator.SetActive(false);
	}

	public void OnSpawnWord()
	{
		cover.transform.SetAsLastSibling();
		topBanner.transform.SetAsLastSibling();
	}

	public void OnStageComplete(int newStage)
	{
		stage.text = "0" + newStage.ToString();
	}

	public void OnWordSolved(int amount)
	{
		int newScore = amount;
		score.text = newScore.ToString();
	}

	public void DisplayBaseline()
	{
        float newYAddition = ((gameController.WordPrefab.GetComponent<RectTransform>().rect.height + gameController.WordsYOffset) * (8 - gameController.MaximumNumLines));

        baseLine.transform.localPosition = new Vector3(baseLine.transform.localPosition.x, startingBaselineY + newYAddition);
    }

	public void DisplayHacks()
	{
		hack.transform.SetAsLastSibling();
		hack.SetActive(true);
	}

	public void DisplayAbility(string text, int uses)
	{
		ability.text = text;
		if(uses > 0){
			for(int i = 0;i < uses;i++){
				GameObject abilityUse = Instantiate(abilityPrefab,abilityParent.transform);
				abilityUse.transform.localPosition = new Vector3(-852 + (i*13),-380,0);
				abilityUses.Add(abilityUse);
			}
		}
	}

	public void LowerDiplayAbility()
	{
		GameObject temp = abilityUses[abilityUses.Count-1];
		temp.GetComponent<Ability>().UseAbility();
		abilityUses.Remove(temp);
	}

	public void CompleteGame()
	{
		gameController.ChangeSubState(Constants.SubState.Complete);
		complete.transform.SetAsLastSibling();
		complete.SetActive(true);
        if (GameManager.Instance.PlayMode == Constants.PlayMode.Multi)
        {
            room.SetActive(true);
        }
		pointsCompleted.text = score.text;
		stageCompleted.text = stage.text;
		List<Hack> hacks = HacksManager.Instance.ActivatedHacks;

		for(int i = 0; i < hacks.Count; i++)
		{
			displayHacks[i].GetComponent<Image>().sprite = hackCompleted;
			displayHacks[i].GetComponent<DisplayHack>().SetDiplayHack(hacks[i]);
		}
	}

	public void CoverWords(int numLinesShowing)
	{
		if(numLinesShowing != 0)
		{
			cover.transform.localPosition = new Vector3(cover.transform.localPosition.x, 460);
			float newYAddition = (((gameController.WordPrefab.GetComponent<RectTransform>().rect.height + gameController.WordsYOffset) * (8 - gameController.MaximumNumLines)) + (gameController.WordPrefab.GetComponent<RectTransform>().rect.height + gameController.WordsYOffset) * (numLinesShowing - 1));
			cover.SetActive(true);
			cover.transform.localPosition = new Vector3(cover.transform.localPosition.x, cover.transform.localPosition.y + newYAddition);	
		}
	}

	public void PauseGame()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");

		if (pauseButton.isOn)
		{
			gameController.ChangeSubState(Constants.SubState.Pause);
			pause.transform.SetAsLastSibling();
			pause.SetActive(true);
			timer.enabled = false;
		}
		else
		{
			gameController.ChangeSubState(Constants.SubState.Playing);
			pause.SetActive(false);
			timer.enabled = true;
		}
	}

	public void OnToggleHelp()
	{
		if (helpButton.isOn)
		{
			gameController.ChangeSubState(Constants.SubState.Pause);
			tutorial.transform.SetAsLastSibling();
			tutorial.SetActive(true);
			timer.enabled = false;
			secondHelpToggle.isOn = true;
		}
		else
		{
			gameController.ChangeSubState(Constants.SubState.Playing);
			tutorial.SetActive(false);
			timer.enabled = true;
			secondHelpToggle.isOn = false;
		}
	}

	public void OnSecondToggleHelp()
	{
		if (secondHelpToggle.isOn)
		{
			gameController.ChangeSubState(Constants.SubState.Pause);
			tutorial.transform.SetAsLastSibling();
			tutorial.SetActive(true);
			timer.enabled = false;
			helpButton.isOn = true;
		}
		else
		{
			gameController.ChangeSubState(Constants.SubState.Playing);
			tutorial.SetActive(false);
			timer.enabled = true;
			helpButton.isOn = false;
		}
	}

	public void ResetTimer()
	{
		timer.Rebind();
		timer.Update(0f);
	}

	public void ResumeGame()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");
		pauseButton.isOn = false;
	}

	public void RestartGame()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");
		pauseButton.isOn = false;
		gameController.NewGame();
	}


	public void ToMainMenu()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");
		pauseButton.isOn = false;
		gameController.NewGame();

        if (GameManager.Instance.PlayMode == Constants.PlayMode.Multi)
        {
            
            PhotonController.Instance.LeaveRoom();
        } else
        {
            GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);

        }

    }

    public void ActivateNegativeHack()
    {
        string hack = HacksManager.Instance.GenerateHacks()[0].GetType().ToString();
        Debug.Log(HacksManager.Instance.GetInstanceID());
        PhotonController.Instance.SendPhotonEvent(Constants.HackSelectedEventCode, hack, Photon.Realtime.ReceiverGroup.All);
    }

    public void ActivatePositiveHack()
    {
        HacksManager.Instance.AddHack(HacksManager.Instance.GenerateHacks()[1]);
    }

    // Moved to End controller
    public void ToRoom()
    {
        GameManager.Instance.ChangeState(Constants.GameStates.Room);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);


    }
}
