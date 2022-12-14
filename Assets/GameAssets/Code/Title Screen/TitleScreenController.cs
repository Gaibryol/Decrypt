using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
	[SerializeField] private Button startButton;
	[SerializeField] private Button creditsButton;
	[SerializeField] private Button quitButton;

	[SerializeField] private GameObject selector;

	[SerializeField] private float yOffset;

	private void StartGame()
	{
		GameManager.Instance.ChangeState(Constants.GameStates.Game);

		SoundEffectsManager.Instance.PlayOneShotSFX("StartGame");
	}

	private void OpenCredits()
	{
		GameManager.Instance.ChangeState(Constants.GameStates.Credits);

		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");
	}

	private void ExitGame()
	{
		Application.Quit();
	}

	public void OnHoverEnter(GameObject obj)
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("Hover");
		selector.SetActive(true);

		selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, obj.transform.localPosition.y + yOffset);
	}

	public void OnHoverExit(GameObject obj)
	{
		selector.SetActive(false);
	}

	private void OnEnable()
	{
		startButton.onClick.AddListener(StartGame);
		creditsButton.onClick.AddListener(OpenCredits);
		quitButton.onClick.AddListener(ExitGame);

		selector.SetActive(false);
	}

	private void OnDisable()
	{
		startButton.onClick.RemoveListener(StartGame);
		creditsButton.onClick.RemoveListener(OpenCredits);
		quitButton.onClick.RemoveListener(ExitGame);

		selector.SetActive(false);
	}
}
