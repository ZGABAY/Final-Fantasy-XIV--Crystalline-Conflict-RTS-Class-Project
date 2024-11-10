using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureButtonController : MonoBehaviour
{
    public delegate void CreateStructure(ScriptableObject structure);
    public static event CreateStructure OnCreateStructure;

    public void CreateStructureButton(ScriptableObject structure)
    {
        // Trigger the event
        OnCreateStructure?.Invoke(structure);
    }
}
