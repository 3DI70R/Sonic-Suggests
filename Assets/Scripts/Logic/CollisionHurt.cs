using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHurt : MonoBehaviour {

    private void OnCollisionStay(Collision c) {
        var player = c.collider.GetComponent<IPlayerHittable>();

        if(player != null) {
            player.OnPlayerHit(gameObject, false);
        }
    }
}
