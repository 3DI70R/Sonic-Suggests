using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetTrigger : TriggerAction {

    public AIController controller;
    public GameObject target;
    public bool oneTime = true;
    public float delay;

    private bool used;

    private void OnTriggerEnter(Collider collider) {

        if (oneTime && used)
        {
            return;
        }
        
        if(collider.GetComponent<PlayerController>())
        {
            StartCoroutine(ApplyTarget());
            used = true;
        }
    }

    private IEnumerator ApplyTarget()
    {
        yield return new WaitForSeconds(delay);
        controller.target = target;
    }
}
