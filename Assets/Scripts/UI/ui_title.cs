using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ui_title : MonoBehaviour
{
	void Start()
	{

	}

	public void btn_play_Click()
	{
		BattleManager.ResetGame();

		SceneManager.LoadScene("corridor", LoadSceneMode.Single);
	}

	public void btn_exit_Click()
	{
		Application.Quit();
	}
};