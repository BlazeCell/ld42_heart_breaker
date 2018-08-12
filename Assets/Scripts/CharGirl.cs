using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class CharGirl : MonoBehaviour
{
	private Animator _animator;
	private NavMeshAgent _nav_agent;

	private NavMeshPath _path;

	private float _last_calc_time = -1.0f;

	private bool _attacking = false;
	private bool _springing_back = false;

	void Start()
	{
		_animator = GetComponent<Animator>();
		_nav_agent = GetComponent<NavMeshAgent>();

		StartCoroutine(CalcPath());
	}
	
	void Update()
	{
		// _nav_agent.updatePosition = false;
		// _nav_agent.updateRotation = true;
		 
		_nav_agent.nextPosition = transform.position;

		if (Time.time > _last_calc_time + 0.5f)
		{
			StartCoroutine(CalcPath());
		}
	}

	private IEnumerator CalcPath()
	{
		while (CharPlayer.instance == null)
			yield return 0;

		// _path = new NavMeshPath();
		// _nav_agent.CalculatePath(CharPlayer.instance.transform.position, _path);

		_nav_agent.SetDestination(CharPlayer.instance.transform.position);

		_last_calc_time = Time.time;
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