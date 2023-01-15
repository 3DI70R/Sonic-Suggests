using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class StartGame : MonoBehaviour
{
	public PlayableDirector Director;
	public Text startText;
	private bool played;

	private void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	public void Update()
	{
		if (!played && CrossPlatformInputManager.GetButton("Submit"))
		{
			Play();
		}
	}

	public void OnStartClicked()
	{
		Play();
	}

	public void Play()
	{
		
		Director.Play();
		played = true;
	}
}
