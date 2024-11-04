using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public Unit unitData;
    public LayerMask ground;

    bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public Transform isSelectedVisual;

    public float agentBaseOffset = 3.0f;

    public NavMeshAgent agent;

    public int raycaseDistance = 1000;

    private void OnEnable()
    {
        SelectionManager.Instance.allSelectables.Add(gameObject);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitData = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSelected)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, raycaseDistance, ground))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (unitData.data.Type == UnitType.Aircraft && IsSelected)
        {
            if (agent.remainingDistance <= 0)
            {
                transform.GetComponent<Aircraft>().StartEngine(false);
            }
            else
            {
                transform.GetComponent<Aircraft>().StartEngine(true);
                agent.baseOffset = 10;
            }
        }

    }
}