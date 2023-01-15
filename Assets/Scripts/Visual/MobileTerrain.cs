using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTerrain : MonoBehaviour
{
	public Terrain terrain;
	
	public void Start()
	{
		#if PLATFORM_ANDROID
		terrain.detailObjectDensity = 0.25f;
		#endif
	}
}
