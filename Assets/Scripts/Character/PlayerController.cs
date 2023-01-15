using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter character;
    public Camera lookCamera;

	private void Update ()
    {
        if(Input.GetButton("Jump")) {
            character.Jump();
        }

        var euler = lookCamera.transform.eulerAngles;
        var rotation = Quaternion.Euler(0, euler.y, 0) *
                       new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        character.SetDirection(new Vector2(rotation.x, rotation.z));
	}
}
