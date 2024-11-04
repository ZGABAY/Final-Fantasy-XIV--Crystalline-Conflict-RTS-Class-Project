using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Different types of infantry
public enum InfantryType
{
    Soldier,
    Marine,
    ShockTrooper,
    Sniper
}
public class Infantry : Unit
{
    // Specific properties for Infantry
    public InfantryType InfantryType;
}