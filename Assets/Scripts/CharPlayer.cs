using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharPlayer : MonoBehaviour
{
	private Animator _animator;

	private float _last_input_time = -1.0f;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	void Update()
	{
		float input_x = Input.GetAxis("Horizontal");
		float input_y = Input.GetAxis("Vertical");

		if (Time.time - _last_input_time >= 0.5f)
		{
			if (Mathf.Abs(input_y) > Mathf.Abs(input_x))
			{
				if (input_y > 0.0f)
				{
					_animator.SetTrigger("attack_fore");

					_last_input_time = Time.time;
				}
				else
				if (input_y < 0.0f)
				{
					_animator.SetTrigger("attack_back");

					_last_input_time = Time.time;
				}
			}
			else
			{
				if (input_x > 0.0f)
				{
					_animator.SetTrigger("attack_right");

					_last_input_time = Time.time;
				}
				else
				if (input_x < 0.0f)
				{
					_animator.SetTrigger("attack_left");

					_last_input_time = Time.time;
				}
			}
		}
	}
};