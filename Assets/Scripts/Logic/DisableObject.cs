using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{

	public GameObject objectToDisable;

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerController>())
		{
			objectToDisable.SetActive(false);
		}
	}
}
