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
	private List<Collider> _lst_colliders = new List<Collider>();

	public UnityEvent_Collider onTrigger;
	
	void OnTriggerEnter(Collider collider)
	{
		_lst_colliders.Add(collider);
	}

	void LateUpdate()
	{
		if (_lst_colliders.Count > 0)
		{
			onTrigger.Invoke(_lst_colliders);
			_lst_colliders.Clear();
		}
	}
};