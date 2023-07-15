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
            GameState state = GameState.Instance;
            int checkpointNumber = int.Parse(spawnPoint.name.Substring(2));
            state.Checkpoint = checkpointNumber;

            if (checkpointNumber == 5)
                state.CurrentState = State.FightingBoss;

            used = true;
        }
    }
}
