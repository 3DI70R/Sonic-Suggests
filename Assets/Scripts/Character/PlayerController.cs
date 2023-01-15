using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter character;
    public Camera lookCamera;

	private void Update ()
    {
        if(CrossPlatformInputManager.GetButton("Jump")) {
            character.Jump();
        }

        var euler = lookCamera.transform.eulerAngles;
        var rotation = Quaternion.Euler(0, euler.y, 0) *
                       new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
        character.SetDirection(new Vector2(rotation.x, rotation.z));
	}
}
