using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
	public AudioClip bgm;
	public float transitionTime = 0.5f;
	public bool oneTime = true;

	private bool used;

	private void OnTriggerEnter(Collider c)
	{
		if (!used && c.GetComponent<PlayerController>())
		{
			used = true;
			MusicManager.Instance.PlayMusic(bgm)
				.SetLooping(true)
				.TransitionCrossFade(transitionTime)
				.ReplaceEverything(true)
				.Start();
		}
	}
}
