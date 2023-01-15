using UnityEngine;

public class DestroyableObject : MonoBehaviour, IHittable
{
    public GameObject destroyedPrefab;

    public void OnHit(PlayerCharacter player)
    {
        Destroy(gameObject);
    }
}