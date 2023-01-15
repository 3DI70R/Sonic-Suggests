using UnityEngine;

public class Motobug : MonoBehaviour, ISpringActivator {

	public void OnSpringActivated(SpringObject spring) {
        var vector = spring.transform.up * spring.springForce;
        character.characterRigidbody.velocity = vector;
	}

	public MovingCharacter character;
    public CharacterAnimator charAnimator;
    private GameObject target;
    private bool isAttacking;
    private float attackTimeout;

	void Update () {
        if(target != null) {
            var direction = target.transform.position - transform.position;
            character.SetVelocity(new Vector2(direction.x, direction.z));
        } else {
            character.SetVelocity(Vector2.zero);
        }
	    
	    charAnimator.animator.SetBool("Attack", attackTimeout > 0);
	    attackTimeout -= Time.deltaTime;
	}

    private void OnCollisionStay(Collision c) {
        var player = c.collider.GetComponent<IPlayerHittable>();

        if(player != null) {
            player.OnPlayerHit(gameObject, false);
            attackTimeout = 0.25f;
        }
    }

    private void OnTriggerEnter(Collider c) {

        var player = c.GetComponent<PlayerCharacter>();

        if(player != null) {
            target = player.gameObject;
        }
    }
}
