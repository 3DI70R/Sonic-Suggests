using UnityEngine;


public class AIController : MonoBehaviour {

    public GameObject target;
    public PlayerCharacter character;

    private void Update() {

        if(!target) {
            character.SetDirection(Vector2.zero);
            return;
        }

        if(target.transform.position.y > transform.position.y + 1) {
            character.Jump();
        }

        var direction = target.transform.position - transform.position;
        character.SetDirection(new Vector2(direction.x, direction.z));
    }
}
