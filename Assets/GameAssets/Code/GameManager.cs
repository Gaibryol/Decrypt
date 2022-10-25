using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField] public Canvas Canvas;
	[SerializeField] public Constants.GameStates GameState;
	[SerializeField] public Texture2D crosshair;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);

		GameState = Constants.GameStates.MainMenu;


    }

    public void ChangeState(Constants.GameStates newState)
	{
		GameState = newState;

        if (GameState == Constants.GameStates.Game)
		{
			SoundEffectsManager.Instance.PlayGameMusic();
            SceneManager.LoadScene("GameScene");
		}
		else if (GameState == Constants.GameStates.MainMenu)
		{
            SoundEffectsManager.Instance.PlayMainMenuMusic();
            SceneManager.LoadScene("TitleScene");
		} else if (GameState == Constants.GameStates.Credits)
        {
            SoundEffectsManager.Instance.PlayMainMenuMusic();
            SceneManager.LoadScene("CreditScene");
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Canvas = FindObjectOfType<Canvas>();
    }
}
