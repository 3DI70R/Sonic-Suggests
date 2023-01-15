using UnityEngine;

public class SpringObject : MonoBehaviour {

    public float springForce;
    public Animator springAnimator;
    public AudioSource source;

    public void OnTriggerEnter(Collider c) {

        if(c.isTrigger) {
            return;
        }

        var springActivator = c.gameObject.GetComponent<ISpringActivator>();

        if(springActivator != null) {
            springActivator.OnSpringActivated(this);
            springAnimator.SetTrigger("Activated");
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Play();
        }
    }
}
