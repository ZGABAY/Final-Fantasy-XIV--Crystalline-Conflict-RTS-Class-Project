using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructruralUIData : MonoBehaviour
{
    // Reference to the UI text element that displays the structural data
    public Text structuralDataText;

    // Example structural data
    private List<string> structuralData;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the structural data
        structuralData = new List<string>
        {
            "Structure 1: Health 100",
            "Structure 2: Health 80",
            "Structure 3: Health 50"
        };

        // Update the UI with the initial data
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Update the UI if the structural data changes
        // In a real scenario, you might have more complex logic to determine when to update the UI
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Simulate a change in structural data
            structuralData[0] = "Structure 1: Health 90";
        }
    }
}