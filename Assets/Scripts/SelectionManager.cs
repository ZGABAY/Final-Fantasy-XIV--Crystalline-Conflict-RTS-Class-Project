using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionManager : RTSSingleton<SelectionManager>
{
    Camera cam;

    public LayerMask groundLayer;
    public LayerMask selectableLayer;

    public GameObject groundMarker;
    public GameObject groundMarkerPrefab;

    public List<GameObject> allSelectables = new List<GameObject>();
    public List<GameObject> currentSelected = new List<GameObject>();

    public bool IsPlacingStructure;
    public int raycaseDistance = 1000;

    private void OnEnable()
    {
        StructureManager.OnPlaceStructure += StructureManager_OnPlaceStructure;
    }

    private void OnDisable()
    {
        StructureManager.OnPlaceStructure -= StructureManager_OnPlaceStructure;
    }

    private void StructureManager_OnPlaceStructure(bool value)
    {
        IsPlacingStructure = value;
    }


    private void OnDestroy()
    {
        allSelectables.Clear();
        currentSelected.Clear();

        allSelectables = null;
        currentSelected = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlacingStructure)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycaseDistance, selectableLayer))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    // select unit
                    SelectByClicking(hit.collider.gameObject);

                    // Check for double click
                    if (DoubleClick())
                    {
                        // select all similar unit types
                        DeselectAll();
                        GroupSelection(hit.collider.transform);
                    }

                }
            }
            else
            {

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    // de-select unit(s)
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycaseDistance, groundLayer))
            {
                groundMarker.transform.position = new Vector3(hit.point.x, groundMarker.transform.position.y, hit.point.z);

                groundMarker.gameObject.SetActive(false);
                groundMarker.gameObject.SetActive(true);

                var gm = GameObject.Instantiate(groundMarkerPrefab,
                                                new Vector3(hit.point.x, hit.point.y, hit.point.z),
                                                Quaternion.identity);

                Destroy(gm, 0.5f);
            }
        }
    }

    private void GroupSelection(Transform transform)
    {
        var unit = transform.GetComponent<Unit>();
        switch (unit.data.Type)
        {
            case UnitType.Infantry:
                {
                    Infantry infantry = (Infantry)unit;
                    switch (infantry.SubType)
                    {
                        case InfantryType.Marine:
                            {
                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.SubType == InfantryType.Marine).ToList();

                                foreach (var unitType in unitTypeList)
                                {
                                    MultiSelect(unitType.gameObject);
                                }
                                break;
                            }
                        case InfantryType.Sniper:
                            {
                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.SubType == InfantryType.Sniper).ToList();

                                foreach (var unitType in unitTypeList)
                                {
                                    MultiSelect(unitType.gameObject);
                                }
                                break;
                            }
                        case InfantryType.ShockTrooper:
                            {
                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.SubType == InfantryType.ShockTrooper).ToList();

                                foreach (var unitType in unitTypeList)
                                {
                                    MultiSelect(unitType.gameObject);
                                }
                                break;
                            }

                    }
                    break;
                }
        }
    }

    private void MultiSelect(GameObject unit)
    {
        if (!currentSelected.Contains(unit))
        {
            currentSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            currentSelected.Remove(unit);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        currentSelected.Add(unit);

        EnableUnitMovement(unit, true);
    }

    public void DeselectAll()
    {
        foreach (var unit in currentSelected)
        {
            SelectUnit(unit, false);
        }

        groundMarker.SetActive(false);

        currentSelected.Clear();
    }

    private void EnableUnitMovement(GameObject unit, bool canMove)
    {
        UnitSelectionIndicator(unit, canMove);
        unit.GetComponent<UnitController>().IsSelected = canMove;
    }

    private void UnitSelectionIndicator(GameObject unit, bool selected)
    {
        // this could should be improved, this was done during the initial
        // demonstration phase
        unit.transform.GetChild(1).gameObject.SetActive(selected);
    }

    internal void DragSelect(GameObject unit)
    {
        if (!currentSelected.Contains(unit))
        {
            currentSelected.Add(unit);

            SelectUnit(unit, true);
        }
    }

    private void SelectUnit(GameObject unit, bool IsSelected)
    {
        UnitSelectionIndicator(unit, IsSelected);
        EnableUnitMovement(unit, IsSelected);
    }

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    bool DoubleClick()
    {
        clicked++;
        if (clicked == 1) clicktime = Time.time;

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        return false;
    }
}