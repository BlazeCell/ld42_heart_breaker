using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DontDestroy : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public static void DestroyAll()
	{
		DontDestroy[] arr_immortals = FindObjectsOfType<DontDestroy>();

		foreach (DontDestroy immortal in arr_immortals)
		{
			Destroy(immortal.gameObject);
		}
	}
};