using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class ResetSkip : MonoBehaviour
{
    public GameObject panel;

    public string buttonToPress;

    public string sceneToLoad;

    private bool _previousPressed;

    private int _confirmationTimer;

    // Update is called once per frame
    void Update()
    {
        if (_confirmationTimer == 0)
            panel.SetActive(false);

        bool currentPressed = Input.GetButton(buttonToPress);
        
        if (currentPressed != _previousPressed && currentPressed)
        {
            if (panel.activeSelf)
            {
                GameState.Instance.IsLoading = true;
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                _confirmationTimer = 180; // 3 seconds
                panel.SetActive(true);
            }
        }

        _previousPressed = currentPressed;
        
        if(_confirmationTimer > 0)
            _confirmationTimer--;
    }
}
