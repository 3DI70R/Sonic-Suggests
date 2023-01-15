﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public MovingCharacter character;
    public Animator animator;
    public GameObject rotationObject;

    public void Update()
    {
        var velocity = character.characterRigidbody.velocity;
        
        animator.SetFloat("XZ Velocity", new Vector3(velocity.x, 0, velocity.z).magnitude);
        animator.SetFloat("Y Velocity", velocity.y);
        animator.SetBool("On Ground", character.OnGround);
        
        rotationObject.transform.rotation = Quaternion.Lerp(rotationObject.transform.rotation,
            Quaternion.LookRotation(character.Direction), Time.deltaTime * 25f);
    }
}
