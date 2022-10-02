using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager Instance { get; private set; }

	private bool musicState;
	private bool soundState;

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

		musicState = true;
		soundState = true;
	}

	public void ToggleMusic(bool isOn)
	{
		musicState = isOn;
	}

	public void ToggleSound(bool isOn)
	{
		soundState = isOn;
	}
}