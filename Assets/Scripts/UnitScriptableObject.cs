using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitType", menuName = "RTS/UnitType", order = 1)]
public class UnitScriptableObject : ScriptableObject
{
    public UnitType Type = UnitType.Infantry;
    public InfantryType infantryType = InfantryType.Marine;
}
