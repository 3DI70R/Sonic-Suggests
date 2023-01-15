using UnityEngine;

public class PlayerPickUp : MonoBehaviour {

    public GameObject collectedEffect;
    public GameObject collectedEffectCenter;
    public float collectDelay;
    public float lifeTime;

    private float time;

    private void Update() {
        time += Time.deltaTime;

        if(lifeTime != 0 && time > lifeTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider collider) {

        if(time < collectDelay) {
            return;
        }

        var collector = collider.gameObject.GetComponent<IPickUpCollector>();

        if(collector != null) {
            collector.OnPickUpCollected(gameObject);

            if(collectedEffect) {
                Instantiate(collectedEffect, 
                    collectedEffectCenter.transform.position, 
                    collectedEffectCenter.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
