using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMusic : MonoBehaviour {

	public void FixedUpdate()
	{
		MusicManager.Instance.MasterVolume = Mathf.Lerp(MusicManager.Instance.MasterVolume, 0, Time.fixedDeltaTime);
	}
}
