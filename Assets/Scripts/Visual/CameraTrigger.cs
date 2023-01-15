using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private static CameraTrigger activeTrigger;

    public CinemachineVirtualCamera cinemachineCamera;
    public int inactivePriority = 10;
    public int activePriority = 20;

    private void Awake()
    {
        DisableCamera();
    }

    public void OnTriggerEnter(Collider c)
    {
        var player = c.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            if (activeTrigger != null)
            {
                activeTrigger.DisableCamera();
            }
            
            EnableCamera();
            activeTrigger = this;
        }
    }

    private void EnableCamera()
    {
        cinemachineCamera.Priority = activePriority;
    }

    private void DisableCamera()
    {
        cinemachineCamera.Priority = inactivePriority;
    }
}
