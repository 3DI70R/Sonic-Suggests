using UnityEngine;

public interface IPlayerHittable {
    void OnPlayerHit(GameObject enemy, bool ignoreProtection);
}
