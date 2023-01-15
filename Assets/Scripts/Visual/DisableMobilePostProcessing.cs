using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DisableMobilePostProcessing : MonoBehaviour
{
	public PostProcessingBehaviour postProcessing;
	
	void Start ()
	{
		#if PLATFORM_ANDROID
				postProcessing.enabled = false;
		#endif
	}
}
