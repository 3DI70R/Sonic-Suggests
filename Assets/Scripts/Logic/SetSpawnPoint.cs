using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    public GameObject spawnPoint;
    private bool used;
    
    public void OnTriggerEnter(Collider c)
    {
        if (!used && c.GetComponent<PlayerController>())
        {
            c.GetComponent<PlayerCharacter>().respawnPoint = spawnPoint;
            used = true;
        }
    }
}
