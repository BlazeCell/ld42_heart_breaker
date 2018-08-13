using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class UnityEvent_Collider : UnityEvent<List<Collider>>
{

};

public class DetectTrigger : MonoBehaviour
{
	public UnityEvent_Collider onTrigger;
	
	public List<Collider> lst_colliders = new List<Collider>();
	
	private bool _event_invoked = false;
	
	void OnTriggerEnter(Collider collider)
	{
		if (_event_invoked)
		{
			_event_invoked = false;
			lst_colliders.Clear();
		}

		lst_colliders.Add(collider);
	}

	void LateUpdate()
	{
		if (lst_colliders.Count > 0)
		{
			onTrigger.Invoke(lst_colliders);
			_event_invoked = true;
		}
	}
};