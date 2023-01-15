using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : TriggerAction
{
	public AudioClip clip;
	public AudioSource source;
	public bool oneTime = true;
	private bool used;
	public float delay;
	
	private void OnTriggerEnter(Collider collider) {
		if(collider.GetComponent<PlayerController>()) {
			StartCoroutine(Play());
		}
	}

	public override void OnAction()
	{
		base.OnAction();

		StartCoroutine(Play());
	}

	private IEnumerator Play()
	{
		if (!oneTime || !used)
		{
			if (delay != 0)
			{
				yield return new WaitForSeconds(delay);
			}
			
			source.PlayOneShot(clip);
			used = true;
		}
	}
}
