using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LookAtCamera : MonoBehaviour
{
	void Start()
	{
		LookAt();
	}
	
	void Update()
	{
		LookAt();
	}

	private void LookAt()
	{
		Vector3 dir = Camera.main.transform.position - transform.position;
		dir.Normalize();

		transform.rotation = Camera.main.transform.rotation;
	}
};