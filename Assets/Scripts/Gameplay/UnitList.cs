using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Units", menuName = "Scriptable/UnitList", order = 0)]
public class UnitList : ScriptableObject
{



}

[System.Serializable]
public class UnitInfo
{
    public string Name;
    public string Code;
    public int Cost;

    public TechTier Tier;
    public GameObject Building;

    public bool CanUpgrade;
    public bool CanBuild;
}
