using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
	void Start()
	{
		
	}
	
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Debug.Log("Quitting...");
			
			Application.Quit();
		}
	}
};