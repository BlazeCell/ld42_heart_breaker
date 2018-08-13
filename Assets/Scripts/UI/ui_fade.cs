using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ui_fade : MonoBehaviour
{
	private static ui_fade _instance;
	public static string header_text = "LEVEL COMPLETE!";
	public static bool show_btn_resume = true;
	public static bool show_btn_next = true;
	public static bool show_btn_retry = true;
	public static bool show_btn_main_menu = true;

	public Text txt_header;
	public Text txt_score;
	public Button btn_resume;
	public Button btn_next;
	public Button btn_retry;
	public Button btn_main_menu;

	void Start()
	{
		_instance = this;

		txt_header.text = header_text;
		btn_resume.gameObject.SetActive(show_btn_resume);
		btn_next.gameObject.SetActive(show_btn_next);
		btn_retry.gameObject.SetActive(show_btn_retry);
		btn_main_menu.gameObject.SetActive(show_btn_main_menu);

		txt_score.text = BattleManager.score.ToString();
	}

	void Update()
	{
		txt_score.text = BattleManager.score.ToString();
	}

	void OnDestroy()
	{
		if (_instance == this)
			_instance = null;
	}

	public static void Load()
	{
		if (_instance == null)
		{
			SceneManager.LoadSceneAsync("ui_fade", LoadSceneMode.Additive);
		}
	}

	public void btn_resume_Click()
	{
		Time.timeScale = 1.0f;

		Destroy(this.gameObject);
	}

	public void btn_next_Click()
	{
		Time.timeScale = 1.0f;

		BattleManager.level += 1;

		SceneManager.LoadScene("corridor", LoadSceneMode.Single);
	}

	public void btn_retry_Click()
	{
		Time.timeScale = 1.0f;

		BattleManager.ResetGame();

		SceneManager.LoadScene("corridor", LoadSceneMode.Single);
	}

	public void btn_main_menu_Click()
	{
		Time.timeScale = 1.0f;

		DontDestroy.DestroyAll();

		SceneManager.LoadScene("title", LoadSceneMode.Single);
	}
};