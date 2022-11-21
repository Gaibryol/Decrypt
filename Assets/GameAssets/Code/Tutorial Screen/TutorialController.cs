using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
	[SerializeField] private Button returnButton;

	private void OnReturnPressed()
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");
		GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);
	}

	private void OnEnable()
	{
		returnButton.onClick.AddListener(OnReturnPressed);
	}

	private void OnDisable()
	{
		returnButton.onClick.RemoveListener(OnReturnPressed);
	}
}
