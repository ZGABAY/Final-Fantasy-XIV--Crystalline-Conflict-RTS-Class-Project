using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStructure", menuName = "RTS/Structure")]
public class StructureScriptableObject : ScriptableObject
{
    public StructureData data;
    public GameObject structurePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization logic if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }
}

[System.Serializable]
public class StructureData
{
    public string Name;
    public StructureType Type;
    public int Cost;
    public float BuildTime;
    // Add other relevant fields
}

public enum StructureType
{
    Barracks,
    WarFactory,
    Airfield,
    PowerPlant,
    TechLab,
    RadarStation,
    CommandCenter
}
