using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
	public GameObject objectToDisable;
	public bool isEnable;

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerController>())
		{
			objectToDisable.SetActive(isEnable);
		}
	}
}
