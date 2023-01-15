using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : TriggerAction
{
	public AudioClip clip;
	public AudioSource source;
	public bool oneTime = true;

	private bool used;
	
	private void OnTriggerEnter(Collider collider) {

		if (oneTime && used)
		{
			return;
		}
		
		if(collider.GetComponent<PlayerController>()) {
			source.PlayOneShot(clip);
			used = true;
		}
	}

	public override void OnAction()
	{
		base.OnAction();
		
		if (oneTime && used)
		{
			return;
		}
		
		source.PlayOneShot(clip);
		used = true;
	}
}
