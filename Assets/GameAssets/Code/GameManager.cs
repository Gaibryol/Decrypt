using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Canvas Canvas;
	public Constants.GameStates GameState;

	public GameObject WordPrefab;

	private List<GameObject> lines;

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
	}

	public void CorrectWord(GameObject word)
	{
		if (lines.IndexOf(word) == 0)
		{
			lines.Remove(word);

			// Need to bump all the other words down
			foreach (GameObject obj in lines)
			{
				obj.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y - word.GetComponent<RectTransform>().rect.height);
			}
		}

		Destroy(word);
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

		bottomLinePos = Camera.main.WorldToViewportPoint(bottomLinePos);

		SpawnWord();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SpawnWord();
		}
	}
}
