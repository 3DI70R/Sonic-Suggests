using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartGame : MonoBehaviour
{
	public PlayableDirector Director;
	private bool played;

	public void Update()
	{
		if (!played && Input.GetButton("Submit"))
		{
			Director.Play();
			played = true;
		}
	}
}
