using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public MovingCharacter character;
    public Animator animator;
    public GameObject rotationObject;
    public bool hurt;

    public bool onlyVelocity = true;

    public void Update()
    {
        var velocity = character.characterRigidbody.velocity;

        animator.SetFloat("XZ Velocity", new Vector3(velocity.x, 0, velocity.z).magnitude);

        if (!onlyVelocity)
        {
            animator.SetFloat("Y Velocity", velocity.y);
            animator.SetBool("On Ground", character.OnGround);
            animator.SetBool("Hurt", hurt);
        }
        
        rotationObject.transform.rotation = Quaternion.Lerp(rotationObject.transform.rotation,
            Quaternion.LookRotation(character.Direction) * Quaternion.Euler(0, 0, -character.Tilt * 35),
                Time.deltaTime * 25f);
    }

    public void Kill() {
        animator.SetTrigger("Dead");
        animator.SetBool("Jump", false);
        animator.SetBool("Normal", false);
    }

    public void Normal() {
        animator.SetTrigger("Normal");
        animator.SetBool("Jump", false);
        animator.SetBool("Dead", false);
    }

    public void Jump() {
        animator.SetTrigger("Jump");
        animator.SetBool("Normal", false);
        animator.SetBool("Dead", false);
    }
}
