using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetTrigger : TriggerAction {

    public AIController controller;
    public GameObject target;
    public bool oneTime = true;

    private bool used;

    private void OnTriggerEnter(Collider collider) {

        if (oneTime && used)
        {
            return;
        }
        
        if(collider.GetComponent<PlayerController>()) {
            controller.target = target;
            used = true;
        }
    }
    
    
}
