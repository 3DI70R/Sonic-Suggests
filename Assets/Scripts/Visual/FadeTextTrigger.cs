using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTextTrigger : MonoBehaviour
{
	public CanvasGroup group;
	private bool entered;

	private void Update()
	{
		group.alpha = Mathf.Lerp(group.alpha, entered ? 1 : 0, Time.deltaTime * 3f);
	}
	
	public void OnTriggerEnter()
	{
		entered = true;
	}

	public void OnTriggerExit()
	{
		entered = false;
	}
}
