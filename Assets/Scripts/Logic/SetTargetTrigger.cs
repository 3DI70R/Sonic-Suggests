using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetTrigger : MonoBehaviour {

    public AIController controller;
    public GameObject target;

    private void OnTriggerEnter(Collider collider) {
        if(collider.GetComponent<PlayerController>()) {
            controller.target = target;
        }
    }
}
