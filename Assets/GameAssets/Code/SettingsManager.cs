using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager Instance { get; private set; }

	[SerializeField, Header("Title Screen")] private Toggle tMusicToggle;
	[SerializeField] private Toggle tSoundToggle;

	[SerializeField, Header("Credit Screen")] private Toggle cMusicToggle;
	[SerializeField] private Toggle cSoundToggle;

	[SerializeField, Header("Game Screen")] private Toggle gMusicToggle;
	[SerializeField] private Toggle gSoundToggle;

	[SerializeField, Header("Audio")] private AudioMixer mixer;
	[SerializeField] private AudioClip theme;
	[SerializeField, Range(-80, 20)] private float normalMusicVolume;
	[SerializeField, Range(-80, 20)] private float normalSoundVolume;

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

		tMusicToggle.onValueChanged.AddListener((isOn) => ToggleMusic(isOn));
		cMusicToggle.onValueChanged.AddListener((isOn) => ToggleMusic(isOn));
		gMusicToggle.onValueChanged.AddListener((isOn) => ToggleMusic(isOn));

		tSoundToggle.onValueChanged.AddListener((isOn) => ToggleSound(isOn));
		cSoundToggle.onValueChanged.AddListener((isOn) => ToggleSound(isOn));
		gSoundToggle.onValueChanged.AddListener((isOn) => ToggleSound(isOn));
	}

	private void Start()
	{
		mixer.SetFloat("Music", normalMusicVolume);
		mixer.SetFloat("Sound", normalSoundVolume);
	}

	private void AdjustUI()
	{
		tMusicToggle.isOn = musicState;
		cMusicToggle.isOn = musicState;
		gMusicToggle.isOn = musicState;

		tSoundToggle.isOn = soundState;
		cSoundToggle.isOn = soundState;
		gSoundToggle.isOn = soundState;
	}

	public void ToggleMusic(bool isOn)
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");

		musicState = isOn;
		mixer.SetFloat("Music", isOn ? normalMusicVolume : -80f);

		AdjustUI();
	}

	public void ToggleSound(bool isOn)
	{
		SoundEffectsManager.Instance.PlayOneShotSFX("ClickSound");

		soundState = isOn;
		mixer.SetFloat("Sound", isOn ? normalSoundVolume : -80f);

		AdjustUI();
	}
}