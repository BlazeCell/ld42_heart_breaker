using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Exit : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		// Debug.Log("Exit Triggered: " + collider.name);

		if(collider.gameObject.tag == "Player")
		{
			if (ui_fade.IsHidden())
			{
				BattleManager.score += 100;

				ui_fade.header_text = "LEVEL\nCOMPLETE!";
				ui_fade.show_btn_resume = false;
				ui_fade.show_btn_next = true;
				ui_fade.show_btn_retry = false;
				ui_fade.show_btn_main_menu = true;
				ui_fade.Load();
			}
		}
	}
};