using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{
    private readonly List<GameObject> currentSelected = new();
    public GameObject groundMarkerPrefab;
    private GameObject selectionCircle;
    public GameObject selectionCirclePrefab;
    private GameObject groundMarker;
    public LayerMask groundLayer;
    public LayerMask unitLayer;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // When the player left clicks
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check for where the mouse ray hit
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                if (groundMarker != null)
                {
                    groundMarker.SetActive(false);
                }
                GameObject clickedUnit = hit.collider.gameObject;
                SelectByClicking(clickedUnit);
            }
            else
            {
                // Deselect all units and deactives the ground marker
                DeselectAll();
                if (groundMarker != null)
                {
                    groundMarker.SetActive(false);
                }
            }
        }

        // When the player right clicks
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check for where the mouse ray hit
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 targetPosition = hit.point;

                // All selected units move to where player clicked
                foreach (var unit in currentSelected)
                {
                    MoveUnitToPosition(unit, targetPosition);
                }

                // Place groundMarker at the target position if there is not one already
                if (groundMarker == null)
                {
                    groundMarker = Instantiate(groundMarkerPrefab);
                }
                groundMarker.transform.position = targetPosition + Vector3.up * 0.1f;
                groundMarker.SetActive(true);

            }
        }
    }

    private void GroupSelection(Transform transform)
    {
        Debug.Log("Group selection: " + transform.name);

        var unit = transform.GetComponent<Unit>();
        switch (unit.Type)
        {
            case UnitType.Infantry:
                {
                    Infantry infantry = (Infantry)unit;
                    switch (infantry.InfantryType)
                    {
                        case InfantryType.Marine:
                            {
                                Debug.Log("Marine selected");

                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.InfantryType == InfantryType.Marine).ToList();

                                foreach (var unitType in unitTypeList)
                                {
                                    MultiSelect(unitType.gameObject);
                                }
                                break;
                            }
                        case InfantryType.Sniper:
                            {
                                Debug.Log("Sniper selected");

                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.InfantryType == InfantryType.Sniper).ToList();

                                foreach (var unitType in unitTypeList)
                                {
                                    MultiSelect(unitType.gameObject);
                                }
                                break;
                            }
                        case InfantryType.ShockTrooper:
                            {
                                Debug.Log("Shock Trooper selected");

                                var unitTypeList = GameObject.FindObjectsOfType<Infantry>().Where(x => x.InfantryType == InfantryType.ShockTrooper).ToList();

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

    private void MultiSelect(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    private void MoveUnitToPosition(GameObject unit, Vector3 targetPosition)
    {
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            Debug.LogError("No NavMeshAgent component found on " + unit.name);
        }
    }

    public void SelectByClicking(GameObject unit)
    {
        Debug.Log("Selected unit: " + unit.name);

        DeselectAll();
        currentSelected.Add(unit);
        EnableUnitMovement(unit, true);

        // Create selection circle above unit(s)
        selectionCircle = Instantiate(selectionCirclePrefab, unit.transform);
        selectionCircle.transform.localPosition = Vector3.zero + Vector3.up * 1.2f; // Position it at the unit's position
        unit.GetComponent<UnitController>().selectionCircle = selectionCircle; // Assuming UnitController script        
    }

    private void DeselectAll()
    {
        foreach (var unit in currentSelected)
        {
            // Remove selection circle
            var unitController = unit.GetComponent<UnitController>();
            if (unitController != null && unitController.selectionCircle != null)
            {
                Destroy(unitController.selectionCircle);
                unitController.selectionCircle = null;
            }
        }
        currentSelected.Clear();
        groundMarkerPrefab.SetActive(false);
    }

    private void EnableUnitMovement(GameObject unit, bool enable)
    {
        Debug.Log("Unit movement enabled: " + enable);

        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = enable;
        }
    }
}
