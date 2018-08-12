using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ManagerMusic : MonoBehaviour
{
	private static ManagerMusic _instance;

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
};