﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharPlayer : MonoBehaviour
{
	public static CharPlayer instance;

	public Camera camera;
	public SphereCollider burst_collider;
	public bool accept_input = true;
	public float turn_speed = 270.0f;

	private Animator _animator;

	private float _last_input_time = -1.0f;

	private Plane _plane_ground = new Plane(Vector3.up, 0.0f);
	private Vector3 _lookat_target = Vector3.zero;

	private bool _attacking = false;
	private bool _springing_back = false;
	private bool _just_finished_attacking = false;

	void Start()
	{
		instance = this;

		_animator = GetComponent<Animator>();

		StartCoroutine(UpdateAsync());
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}
	
	private IEnumerator UpdateAsync()
	{
		yield return new WaitForSeconds(0.0f);

		if (_just_finished_attacking)
		{
			_just_finished_attacking = false;
			yield return new WaitForSeconds(0.05f);
		}

		if (accept_input)
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			float intersect = 0.0f;
			if (_plane_ground.Raycast(ray, out intersect))
			{
				// Get the point that is clicked
				_lookat_target = ray.GetPoint(intersect);
			}

			if (!_attacking)
			{
				Vector3 lookat_dir = _lookat_target - transform.position;
				lookat_dir.y = 0.0f;
				lookat_dir.Normalize();

				// Debug.Log("lookat_dir: "+lookat_dir);

				float lookat_angle =  Vector3.SignedAngle(Vector3.forward, lookat_dir, Vector3.up);
				Quaternion rot_target = Quaternion.AngleAxis(lookat_angle, Vector3.up);

				// Debug.Log("lookat_angle: "+lookat_angle);

				// transform.rotation = Quaternion.RotateTowards(transform.rotation, rot_target, Time.deltaTime * turn_speed);
				transform.rotation = rot_target;
			}

			if (!_attacking)
			{
				if (Input.GetMouseButton(0))
				{
					_animator.SetTrigger("attack_fore");

					_attacking = true;
				}
			}
		}

		StartCoroutine(UpdateAsync());
	}

	public void IdleStart()
	{
		if (_attacking)
			_just_finished_attacking = true;
		_attacking = false;
		_springing_back = false;
		_animator.SetBool("spring_back", false);

		Vector3 pos = transform.position;
		pos.y = 0.0f;

		// Ensure that we don't clip thru walls.
		if (   Mathf.Abs(pos.x) > 10.0f
			&& Mathf.Abs(pos.z) > 10.0f)
		{
			if (Mathf.Abs(pos.x) < Mathf.Abs(pos.z))
				pos.x = Mathf.Clamp(pos.x, -9.5f, 9.5f);
			else
				pos.z = Mathf.Clamp(pos.z, -9.5f, 9.5f);
		}

		transform.position = pos;
	}

	public void PunchContact(List<Collider> lst_colliders)
	{
		if (   _attacking
			&& !_springing_back)
		{
			foreach (Collider collider in lst_colliders)
			{
				if (collider.gameObject.tag == "Girl")
				{
					CharGirl girl = collider.GetComponent<CharGirl>();

					if (girl != null)
					{
						if (girl.mobile)
						{
							girl.Hit();

							HitAllGirlsWithinBurst();

							_springing_back = true;
							_animator.SetBool("spring_back", true);

							return;
						}
					}
				}
				else
				if (collider.gameObject.tag == "Wall")
				{
					_springing_back = true;
					_animator.SetBool("spring_back", true);
				}
			}
		}
		// Debug.Log("Punched '"+collider.name+"'");
	}

	private void HitAllGirlsWithinBurst()
	{
		float burst_radius = 
			  burst_collider.radius 
			* Mathf.Max(burst_collider.transform.lossyScale.x, burst_collider.transform.lossyScale.z);

		CharGirl[] arr_girls = FindObjectsOfType<CharGirl>();
		foreach (CharGirl girl in arr_girls)
		{
			if (girl.mobile)
			{
				Collider collider = girl.GetComponent<Collider>();
				Vector3 closest_instersect = collider.ClosestPoint(burst_collider.transform.position);
				if (Vector3.Distance(burst_collider.transform.position, closest_instersect) <= burst_radius)
				{
					girl.Hit();
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (!_attacking)
		{
			if (collider.gameObject.tag == "Girl")
			{
				CharGirl girl = collider.GetComponent<CharGirl>();

				if (girl != null)
				{
					if (girl.mobile)
					{
						if (ui_fade.IsHidden())
						{
							switch (UnityEngine.Random.Range(0, 3))
							{
								case 0: ui_fade.header_text = "YOU'VE BEEN\nGLOMPED!";
									break;
								case 1: ui_fade.header_text = "EWW!\nKOOTIES!";
									break;
								case 2: ui_fade.header_text = "NO-SCOPED\nBY CUPID!";
									break;
							}
							
							ui_fade.show_btn_resume = false;
							ui_fade.show_btn_next = false;
							ui_fade.show_btn_retry = true;
							ui_fade.show_btn_main_menu = true;
							ui_fade.Load();

							ManagerMusic.PlayGameOverMusic();
						}
					}
				}
			}
		}
	}
};