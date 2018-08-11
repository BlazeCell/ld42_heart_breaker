using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharPlayer : MonoBehaviour
{
	public Camera camera;
	public float turn_speed = 270.0f;

	private Animator _animator;

	private float _last_input_time = -1.0f;

	private Plane _plane_ground = new Plane(Vector3.up, 0.0f);
	private Vector3 _lookat_target = Vector3.zero;

	private bool _attacking = false;
	private bool _springing_back = false;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}
	
	void Update()
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

			transform.rotation = Quaternion.RotateTowards(transform.rotation, rot_target, Time.deltaTime * turn_speed);
		}

		if (!_attacking)
		{
			if (Input.GetMouseButtonDown(0))
			{
				_animator.SetTrigger("attack_fore");

				_attacking = true;
			}
		}
	}

	public void IdleStart()
	{
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

	public void PunchContact(Collider collider)
	{
		if (   _attacking
			&& !_springing_back)
		{
			if (   collider.gameObject.tag != "Player"
				&& collider.gameObject.tag != "Finish")
			{
				_springing_back = true;
				_animator.SetBool("spring_back", true);
			}
		}
		// Debug.Log("Punched '"+collider.name+"'");
	}
};