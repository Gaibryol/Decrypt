using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager Instance { get; private set; }

	[SerializeField, Header("Game Screen")] private Toggle MusicToggle;
	[SerializeField] private Toggle SoundToggle;

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
            DontDestroyOnLoad(this.gameObject);

        }

        musicState = true;
		soundState = true;

        MusicToggle.onValueChanged.AddListener((isOn) => ToggleMusic(isOn));
        SoundToggle.onValueChanged.AddListener((isOn) => ToggleSound(isOn));


    }

    private void Start()
	{
		mixer.SetFloat("Music", normalMusicVolume);
		mixer.SetFloat("Sound", normalSoundVolume);
	}

	private void AdjustUI()
	{
		MusicToggle.isOn = musicState;
		SoundToggle.isOn = soundState;
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

    private void OnLevelWasLoaded(int level)
    {
        SoundToggle = GameObject.FindGameObjectWithTag("SoundToggle").GetComponent<Toggle>();
        MusicToggle = GameObject.FindGameObjectWithTag("MusicToggle").GetComponent<Toggle>();
        MusicToggle.onValueChanged.AddListener((isOn) => ToggleMusic(isOn));
        SoundToggle.onValueChanged.AddListener((isOn) => ToggleSound(isOn));
        AdjustUI();
    }
}