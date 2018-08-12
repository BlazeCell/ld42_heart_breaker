using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour
{
	public Bounds bounds;

	void Start()
	{
		Collider collider = GetComponent<Collider>();

		bounds = collider.bounds;
	}
};