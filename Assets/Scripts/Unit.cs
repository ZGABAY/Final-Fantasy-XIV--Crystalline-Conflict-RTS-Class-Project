using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Different types of units
public enum UnitType
{
    Infantry,
    Vehicle,
    Aircraft,
    Special
}

public class Unit : MonoBehaviour, IUnit
{
    public string Name;
    public int Health;
    public int AttackDamage;
    public int Armor;
    public int MovementSpeed;

    public UnitType Type;

    public void Attack(Transform target)
    {
        Debug.Log("Attacking");
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        Debug.Log("Died");
        throw new System.NotImplementedException();
    }

    public void MoveToPosition(Vector3 position)
    {
        Debug.Log("Moving");
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
