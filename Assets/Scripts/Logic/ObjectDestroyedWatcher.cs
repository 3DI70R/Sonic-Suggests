using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyedWatcher : MonoBehaviour
{
    public GameObject[] objectList;
    public TriggerAction[] actions;

    private bool goalExecuted;

    private void FixedUpdate()
    {
        if (goalExecuted)
        {
            return;
        }
        
        foreach (var o in objectList)
        {
            if (o != null)
            {
                return;
            }
        }

        goalExecuted = true;

        foreach (var action in actions)
        {
            action.OnAction();
        }
    }
}
