using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField] public Canvas Canvas;
	[SerializeField] public Constants.GameStates GameState;
	[SerializeField] public Texture2D crosshair;

	[SerializeField, Header("Controllers")] private GameController gameController;
	[SerializeField] private TitleScreenController titleController;

	[SerializeField, Header("Screens")] private GameObject titleScreen;
	[SerializeField] private GameObject gameScreen;
	[SerializeField] private GameObject creditsScreen;

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

		Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);

		GameState = Constants.GameStates.MainMenu;
	}

	public void ChangeState(Constants.GameStates newState)
	{
		GameState = newState;

		titleScreen.SetActive(newState == Constants.GameStates.MainMenu);
		gameScreen.SetActive(newState == Constants.GameStates.Game);
		creditsScreen.SetActive(newState == Constants.GameStates.Credits);

		if (GameState == Constants.GameStates.Game)
		{
			SoundEffectsManager.Instance.PlayGameMusic();
			gameController.StartGame();
		}
		else if (GameState == Constants.GameStates.MainMenu)
		{
			SoundEffectsManager.Instance.PlayMainMenuMusic();
		}
	}
}
