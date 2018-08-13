using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AutoSpin : MonoBehaviour
{
	public float angular_speed = 90.0f;

	void Update()
	{
		transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * angular_speed, Vector3.up);
	}
};