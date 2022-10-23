using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersManager : MonoBehaviour
{
	public static LettersManager Instance { get; private set; }

	[SerializeField, Header("Blue")] private List<Sprite> blue;

	[SerializeField, Header("Green")] private List<Sprite> green;

	[SerializeField, Header("Orange")] private List<Sprite> orange;

	[SerializeField, Header("Pink")] private List<Sprite> pink;

	[SerializeField, Header("Purple")] private List<Sprite> purple;

	[SerializeField, Header("Red")] private List<Sprite> red;

	[SerializeField, Header("Yellow")] private List<Sprite> yellow;

	[SerializeField] Sprite tileCover;

    [SerializeField] public GameObject placeHolder;

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
	}

	public Sprite GetSprite(Constants.LetterColors color, string letter)
	{
		int index = char.Parse(letter.ToUpper()) - 64;

		switch (color)
		{
			case Constants.LetterColors.Blue:
				return blue[index - 1];

			case Constants.LetterColors.Green:
				return green[index - 1];

			case Constants.LetterColors.Orange:
				return orange[index - 1];

			case Constants.LetterColors.Pink:
				return pink[index - 1];

			case Constants.LetterColors.Purple:
				return purple[index - 1];

			case Constants.LetterColors.Red:
				return red[index - 1];

			case Constants.LetterColors.Yellow:
				return yellow[index - 1];
		}

		return null;
	}
	public Sprite GetTileCoverSprite(){
		return tileCover;
	}
}
