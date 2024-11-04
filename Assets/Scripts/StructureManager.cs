using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class StructureManager : RTSSingleton<StructureManager>
{
    #region CUSTOM EVENTS
    public delegate void PlaceStructure(bool value);
    public static event PlaceStructure OnPlaceStructure;

    public delegate void AdjustTerrainForStructure(Vector3 point, Transform t);
    public static event AdjustTerrainForStructure OnAdjustTerrainForStructure;
    #endregion

    public GameObject currentStructurePrefab;
    public LayerMask groundLayer;


    public GameObject tempStructure;

    RaycastHit hitInfo = new RaycastHit();
    RaycastHit realtimeHit = new RaycastHit();

    public Mesh tempGeometryMesh;
    public Material transparentMaterial;

    public bool ReadyToPlace;
    public StructureScriptableObject currentStructureSO;

    private void OnEnable()
    {
        StructureButtonController.OnCreateStructure += StructureButtonController_OnCreateStructure;
    }

    private void OnDisable()
    {
        StructureButtonController.OnCreateStructure -= StructureButtonController_OnCreateStructure;
    }

    private void StructureButtonController_OnCreateStructure(ScriptableObject structure)
    {
        // let's handle the structure placement here ...
        currentStructureSO = (StructureScriptableObject)structure;

        if (tempStructure != null)
            Destroy(tempStructure);


        switch (currentStructureSO.data.Type)
        {
            case StructureType.Barracks:
            case StructureType.WarFactory:
            case StructureType.Airfield:
            case StructureType.PowerPlant:
            case StructureType.TechLab:
            case StructureType.RadarStation:
            case StructureType.CommandCenter:
                {
                    currentStructurePrefab = currentStructureSO.structurePrefab;

                    // create the structure
                    tempStructure = GameObject.Instantiate(currentStructurePrefab);
                    var structureData = tempStructure.GetComponent<Structure>();
                    structureData.UpdateData(currentStructureSO, false);

                    tempGeometryMesh = structureData.meshFilter.mesh;

                    ReadyToPlace = true;

                    OnPlaceStructure?.Invoke(ReadyToPlace);

                    break;
                }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ReadyToPlace)
        {
            bool hitT = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out realtimeHit, 100, groundLayer);
            if (hitT)
            {
                tempStructure.GetComponent<Structure>().structure.GetComponent<Renderer>().material = transparentMaterial;
                tempStructure.transform.position = Vector3.Lerp(tempStructure.transform.position,
                                                           new Vector3(realtimeHit.point.x,
                                                                       realtimeHit.point.y,
                                                                       realtimeHit.point.z),
                                                           Time.deltaTime * 10);

                // let's try to control the orientation
                if (Input.GetKey(KeyCode.Z))
                {
                    tempStructure.transform.Rotate(Vector3.up, 1);
                }
                if (Input.GetKey(KeyCode.X))
                {
                    tempStructure.transform.Rotate(Vector3.up, -1);
                }

            }

            if (Input.GetMouseButtonDown(0))
            {
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 100, groundLayer);
                if (hit)
                {

                    Quaternion placementAngle = tempStructure.transform.rotation;

                    if (tempStructure)
                        Destroy(tempStructure);

                    var newStructure = GameObject.Instantiate(currentStructurePrefab);
                    newStructure.name = $"{currentStructureSO.name}";
                    newStructure.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                    newStructure.transform.rotation = placementAngle;

                    ReadyToPlace = false;

                    var structureData = newStructure.GetComponent<Structure>();
                    structureData.UpdateData(currentStructureSO, true);

                    OnPlaceStructure?.Invoke(ReadyToPlace);

                    OnAdjustTerrainForStructure?.Invoke(hitInfo.point, structureData.structureSelectedVisualizer.transform);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (tempStructure)
                    Destroy(tempStructure);

                ReadyToPlace = false;
                OnPlaceStructure?.Invoke(ReadyToPlace);
            }
        }
    }
}