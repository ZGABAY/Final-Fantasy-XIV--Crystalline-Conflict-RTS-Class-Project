using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public bool IsSelected { get; set; } // Bool to track if unit is selected
    private NavMeshAgent navMeshAgent;
    public GameObject selectionCircle = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        IsSelected = false;
        if (selectionCircle != null)
        {
            selectionCircle.SetActive(IsSelected);
        }
    }
}