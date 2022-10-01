using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Canvas Canvas;
	public Constants.GameStates GameState;

	public GameObject WordPrefab;

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
		List<string> words = WordsManager.Instance.GetScrambledWord();

		GameObject newWord = Instantiate(WordPrefab, Canvas.transform);
		newWord.GetComponent<Word>().SpawnWord(words[0], words[1]);
		newWord.transform.position = new Vector3(newWord.transform.position.x - (37.5f * words[0].Length), newWord.transform.position.y);
	}
}
