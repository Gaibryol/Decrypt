using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundEffectsManager : MonoBehaviour
{
	public static SoundEffectsManager Instance { get; private set; }

	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[SerializeField, Header("Music")] private AudioClip mainMenuTheme;
	[SerializeField] private AudioClip gameTheme;
	[SerializeField] private AudioClip everythingElseTheme;

	[SerializeField, Header("Sound Effects")] private AudioClip clickSound;
	[SerializeField] private AudioClip gameEnded; // TODO
	[SerializeField] private AudioClip hackSelected; // TODO
	[SerializeField] private AudioClip hover;
	[SerializeField] private AudioClip pickupLetter;
	[SerializeField] private AudioClip putdownLetter;
	[SerializeField] private AudioClip stageEnded;
	[SerializeField] private AudioClip startGame;
	[SerializeField] private AudioClip warning;
	[SerializeField] private AudioClip wordScored1;
	[SerializeField] private AudioClip wordScored2;
	[SerializeField] private AudioClip wordScored3;
	[SerializeField] private AudioClip wordScored4;
	[SerializeField] private AudioClip wordSpawn;

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

		PlayMainMenuMusic();
	}

	public void PlayOneShotSFX(string soundEffect)
	{
		switch (soundEffect)
		{
			case "ClickSound":
				sfxSource.PlayOneShot(clickSound);
				break;
			case "GameEnded":
				sfxSource.PlayOneShot(gameEnded);
				break;
			case "HackSelected":
				sfxSource.PlayOneShot(hackSelected);
				break;
			case "Hover":
				sfxSource.PlayOneShot(hover);
				break;
			case "PickupLetter":
				sfxSource.PlayOneShot(pickupLetter);
				break;
			case "PutdownLetter":
				sfxSource.PlayOneShot(putdownLetter);
				break;
			case "StageEnded":
				sfxSource.PlayOneShot(stageEnded);
				break;
			case "StartGame":
				sfxSource.PlayOneShot(startGame);
				break;
			case "Warning":
				sfxSource.PlayOneShot(warning);
				break;
			case "WordScore":
				int randNum = (int)Random.Range(1, 5);

				switch (randNum)
				{
					case 1:
						sfxSource.PlayOneShot(wordScored1);
						break;

					case 2:
						sfxSource.PlayOneShot(wordScored2);
						break;

					case 3:
						sfxSource.PlayOneShot(wordScored3);
						break;

					case 4:
						sfxSource.PlayOneShot(wordScored4);
						break;
				}
				break;
			case "WordSpawned":
				sfxSource.PlayOneShot(wordSpawn);
				break;
		}
	}

	public void PlayGameMusic()
	{
		musicSource.clip = gameTheme;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlayMainMenuMusic()
	{
		musicSource.clip = mainMenuTheme;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlayEverythingElseMusic()
	{
		musicSource.clip = everythingElseTheme;
		musicSource.loop = true;
		musicSource.Play();
	}
}