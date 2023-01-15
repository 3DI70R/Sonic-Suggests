using UnityEngine;

public class DestroyableObject : MonoBehaviour, IHittable
{
    public GameObject destroyedPrefab;

    public virtual void OnHit(PlayerCharacter player)
    {
        Destroy(gameObject);
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
    }
}