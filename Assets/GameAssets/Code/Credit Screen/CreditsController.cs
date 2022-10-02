using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
	[SerializeField] private Button returnButton;

	[SerializeField] private Toggle musicToggle;
	[SerializeField] private Toggle soundToggle;

	private void OnReturnPressed()
	{
		GameManager.Instance.ChangeState(Constants.GameStates.MainMenu);
	}

	private void OnEnable()
	{
		returnButton.onClick.AddListener(OnReturnPressed);

		musicToggle.onValueChanged.AddListener((isOn) => SettingsManager.Instance.ToggleMusic(isOn));
		soundToggle.onValueChanged.AddListener((isOn) => SettingsManager.Instance.ToggleSound(isOn));
	}

	private void OnDisable()
	{
		returnButton.onClick.RemoveListener(OnReturnPressed);

		musicToggle.onValueChanged.RemoveListener((isOn) => SettingsManager.Instance.ToggleMusic(isOn));
		soundToggle.onValueChanged.RemoveListener((isOn) => SettingsManager.Instance.ToggleSound(isOn));
	}
}
