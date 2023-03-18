using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartGame : MonoBehaviour
{
	public PlayableDirector Director;
	private bool played;

	private GameState state;

	public void Start()
	{
		state = GameState.Instance;
		state.CurrentState = State.InMenu;
	}

	public void Update()
	{
		state.FrameCounter++;
		
		if (!played && Input.GetButton("Submit"))
		{
			state.CurrentState = State.IntroCutscene;
			Director.Play();
			played = true;
		}
	}
}
