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

		state.IsLoading = false;
		
		state.CurrentState = State.InMenu;
	}

	public void Update()
	{
		if (!played && Input.GetButton("Submit"))
		{
			state.CurrentState = State.IntroCutscene;
			Director.Play();
			played = true;
		}
	}
}
