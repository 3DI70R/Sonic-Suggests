using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutsceneStateUpdater : MonoBehaviour
{
    private GameState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Instance;
        state.IsLoading = false;
        state.CurrentState = State.EndCutscene;
    }
}
