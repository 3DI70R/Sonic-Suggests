using UnityEngine;

public class AttackSound : MonoBehaviour
{
    public AudioSource source;

    public void OnAttack()
    {
        source.pitch = Random.Range(0.85f, 1.15f);
        source.Play();
    }
}
