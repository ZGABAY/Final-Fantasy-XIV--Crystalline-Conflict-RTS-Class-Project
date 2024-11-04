using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public abstract void MoveToPosition(Vector3 position);
    public abstract void Attack(Transform target);
    public abstract void TakeDamage(float damage);
    public abstract void Die();
}
