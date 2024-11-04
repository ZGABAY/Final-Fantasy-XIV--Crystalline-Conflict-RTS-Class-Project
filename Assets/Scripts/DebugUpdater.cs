using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUpdater : MonoBehaviour
{
    private bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DebugUpdater started.");
    }

    // Update is called once per frame
    void Update()
    {

        // Toggle debug mode with D
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleDebugMode();
        }
    }

    void ToggleDebugMode()
    {
        debugMode = !debugMode;
        Debug.Log("Debug mode: " + (debugMode ? "ON" : "OFF"));
    }
}