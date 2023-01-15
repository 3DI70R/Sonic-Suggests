using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUV : MonoBehaviour
{
	public MeshRenderer renderer;

	public void Update()
	{
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(Time.time / 50f, 0f));
		renderer.sharedMaterial.SetTextureOffset("_DetailTex", new Vector2(Time.time / 185f, 0f));
	}
}
