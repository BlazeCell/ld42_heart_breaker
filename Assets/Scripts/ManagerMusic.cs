using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ManagerMusic : MonoBehaviour
{
	private static ManagerMusic _instance;

	public AudioSource audio_battle_music;
	public AudioSource audio_game_over_music;

	void Start()
	{
		_instance = this;
	}

	void OnDestroy()
	{
		if (_instance == this)
			_instance = null;
	}
	
	public static void Load()
	{
		if (_instance == null)
			SceneManager.LoadSceneAsync("manager_music", LoadSceneMode.Additive);
	}

	public static void PlayBattleMusic()
	{
		if (   _instance != null
			&& !_instance.audio_battle_music.isPlaying)
		{
			_instance.audio_game_over_music.Stop();
			_instance.audio_battle_music.Play();
		}
	}

	public static void PlayGameOverMusic()
	{
		if (   _instance != null
			&& !_instance.audio_game_over_music.isPlaying)
		{
			_instance.audio_battle_music.Stop();
			_instance.audio_game_over_music.Play();
		}
	}
};