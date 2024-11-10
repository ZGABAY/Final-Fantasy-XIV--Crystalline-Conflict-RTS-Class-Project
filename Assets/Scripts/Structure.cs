using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public MeshFilter meshFilter;
    public GameObject structure;
    public GameObject structureSelectedVisualizer;

    public List<UnitScriptableObject> structureUnits;  // List of units associated with this structure

    // Event to update the unit view
    public static event Action<StructureScriptableObject> OnUpdateUnitsView;

    // Event to update unit production progress
    public static event Action<UnitScriptableObject, float> OnUpdateUnitsProgress;

    // Initializes the Structure with data from the ScriptableObject
    public void UpdateData(StructureScriptableObject structureSO, bool isPlaced)
    {
        // Set the mesh for visual representation
        if (structureSO.structurePrefab.TryGetComponent(out MeshFilter mesh))
            meshFilter.mesh = mesh.sharedMesh;

        // Assign prefab and visualizer
        structure = structureSO.structurePrefab;
        structureSelectedVisualizer = new GameObject("Structure Visualizer");

        // Load unit data specific to this structure
        structureUnits = structureSO.structureUnits;

        // Trigger the event to update the unit UI
        OnUpdateUnitsView?.Invoke(structureSO);
    }

    // Method to update unit production progress
    public void UpdateUnitProductionProgress(UnitScriptableObject unit, float progress)
    {
        // Invoke the progress event to notify ActiveUnitsUIController
        OnUpdateUnitsProgress?.Invoke(unit, progress);
    }
}
