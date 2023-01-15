using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
	void Update () 
	{
		if (Input.GetButton(""))
		{
			Application.Quit();
		}
	}
}
