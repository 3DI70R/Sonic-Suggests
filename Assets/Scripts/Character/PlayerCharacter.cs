using UnityEngine;

public class PlayerCharacter : MonoBehaviour, ISpringActivator {

    private Collider[] colliders = new Collider[8];

    public MovingCharacter character;
    public CharacterAnimator animator;
    public float jumpVelocity;
    public AudioSource audioSource;

    public AudioClip jumpSound;

    private bool isJumping;
    private bool jump;
    private Vector2 direction;

    private void Update() {
        UpdateMovement();
        CheckForHittable();
    }

    private void CheckForHittable() {
        if(isJumping) {

            var center = character.characterCapsule.transform.TransformPoint(
                    character.characterCapsule.center) + Vector3.down * character.characterCapsule.height / 1.5f;

            var count = Physics.OverlapSphereNonAlloc(center, character.characterCapsule.radius * 1.5f, colliders);

            for(int i = 0; i < count; i++) {
                var c = colliders[i];
                IHittable hittable = c.gameObject.GetComponent<IHittable>();

                if(hittable != null) {

                    if(c.transform.position.y < transform.position.y && character.characterRigidbody.velocity.y < 0) {
                        var v = character.characterRigidbody.velocity;
                        v.y = -v.y * 1.2f;
                        character.characterRigidbody.velocity = v;
                    }

                    hittable.OnHit(this);
                }
            }
        }
    }

    private void UpdateMovement() {
        var v = character.characterRigidbody.velocity;

        if(character.OnGround && isJumping && v.y < 0.1) {
            isJumping = false;
            animator.Normal();
        }

        if(!isJumping && character.OnGround && jump) {
            v.y = jumpVelocity;
            character.characterRigidbody.velocity = v;
            isJumping = true;
            animator.Jump();
            audioSource.PlayOneShot(jumpSound);
        }

        character.SetVelocity(direction);
        jump = false;
    }

    public void SetDirection(Vector2 rotation) {
        direction = rotation;
    }

    public void Jump() {
        jump = true;
    }

    public void OnSpringActivated(SpringObject spring) {
        var vector = spring.transform.up * spring.springForce;
        character.characterRigidbody.velocity = vector;
    }
}
