using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUV : MonoBehaviour
{
	public MeshRenderer renderer;
	public float speed = 1f;

	public void Update()
	{
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2((Time.time * speed) / 50f, 0f));
		renderer.sharedMaterial.SetTextureOffset("_DetailNormalMap", new Vector2((Time.time * speed) / 185f, 0f));
	}
}
