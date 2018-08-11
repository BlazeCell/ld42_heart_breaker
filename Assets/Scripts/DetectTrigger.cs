using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class UnityEvent_Collider : UnityEvent<Collider>
{

};

public class DetectTrigger : MonoBehaviour
{
	public UnityEvent_Collider onTrigger;
	
	void OnTriggerEnter(Collider collider)
	{
		onTrigger.Invoke(collider);
	}
};