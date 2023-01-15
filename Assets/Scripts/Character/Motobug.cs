using UnityEngine;

public class Motobug : MonoBehaviour, ISpringActivator {

	public void OnSpringActivated(SpringObject spring) {
        var vector = spring.transform.up * spring.springForce;
        character.characterRigidbody.velocity = vector;
	}

	public MovingCharacter character;
    private GameObject target;

	void Update () {
        if(target != null) {
            var direction = target.transform.position - transform.position;
            character.SetVelocity(new Vector2(direction.x, direction.z));
        } else {
            character.SetVelocity(Vector2.zero);
        }
	}

    private void OnCollisionStay(Collision c) {
        var player = c.collider.GetComponent<IPlayerHittable>();

        if(player != null) {
            player.OnPlayerHit(gameObject, false);
        }
    }

    private void OnTriggerEnter(Collider c) {

        var player = c.GetComponent<PlayerCharacter>();

        if(player != null) {
            target = player.gameObject;
        }
    }
}
