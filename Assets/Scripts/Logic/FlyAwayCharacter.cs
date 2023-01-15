using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAwayCharacter : MonoBehaviour
{
    public GameObject flyAwayPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();

        if (player != null)
        {
            player.SilentRespawn(3);
            
            if (other.GetComponent<PlayerController>())
            {
                Instantiate(flyAwayPrefab, other.transform.position, Quaternion.identity);
            }
        }
    }
}
