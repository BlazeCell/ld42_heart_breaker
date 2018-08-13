using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class CharGirl : MonoBehaviour
{
	public Animator animator_heart;

	public bool active = true;
	public bool mobile = true;
	public int value = 1;
	[Range(0, 5)]
	public int health = 1;
	public float turn_speed = 180.0f;

	private Animator _animator;
	private NavMeshAgent _nav_agent;

	private float _heart_size = 1.0f;
	private float _target_heart_size = 1.0f;

	private float _last_calc_time = -1.0f;

	private bool _springing_back = false;

	void Start()
	{
		_animator = GetComponent<Animator>();
		_nav_agent = GetComponent<NavMeshAgent>();

		_target_heart_size = health;
		_heart_size = _target_heart_size;

		_animator.SetBool("active", active);
		_animator.SetInteger("health", health);
		animator_heart.SetFloat("size", _heart_size);

		// StartCoroutine(CalcPath());
	}
	
	void Update()
	{
		if (   active)
		{
			if (mobile)
			{
				Vector3 dir_player = CharPlayer.instance.transform.position - transform.position;
				dir_player.Normalize();

				float angle_target = Vector3.SignedAngle(Vector3.forward, dir_player, Vector3.up);

				Quaternion rot_target = Quaternion.AngleAxis(angle_target, Vector3.up);

				transform.rotation = Quaternion.RotateTowards(transform.rotation, rot_target, Time.deltaTime * turn_speed);

				// _nav_agent.updatePosition = false;

				// _nav_agent.nextPosition = transform.position;

				// {
				// 	_nav_agent.SetDestination(CharPlayer.instance.transform.position);
				// }
			}

			// Ensure that we don't clip thru walls.
			Vector3 pos = transform.position;
			if (   Mathf.Abs(pos.x) > 10.0f
				&& Mathf.Abs(pos.z) > 10.0f)
			{
				if (Mathf.Abs(pos.x) < Mathf.Abs(pos.z))
					pos.x = Mathf.Clamp(pos.x, -9.5f, 9.5f);
				else
					pos.z = Mathf.Clamp(pos.z, -9.5f, 9.5f);

				transform.position = pos;
			}

			_heart_size = Mathf.MoveTowards(_heart_size, _target_heart_size, Time.deltaTime * 1.0f);
			animator_heart.SetFloat("size", _heart_size);
		}
	}

	private IEnumerator CalcPath()
	{
		while (CharPlayer.instance == null)
			yield return 0;

		_nav_agent.SetDestination(CharPlayer.instance.transform.position);

		_last_calc_time = Time.time;
	}

	public void Hit()
	{
		if (   active
			&& mobile)
		{
			mobile = false;

			--health;
			_target_heart_size = health;
			animator_heart.SetBool("broken", _target_heart_size < 1.0f);
			animator_heart.SetTrigger("tremor");

			_animator.SetInteger("health", health);
			_animator.SetTrigger("hit");

			if (health <= 0)
				BattleManager.score += value;

			// _nav_agent.ResetPath();
		}
	}

	public void IdleStart()
	{
		if (active)
		{
			mobile = true;

			Vector3 pos = transform.position;
			pos.y = 0.0f;

			transform.position = pos;
		}
	}

	public void PunchContact(Collider collider)
	{
		if (   active
			&& mobile)
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