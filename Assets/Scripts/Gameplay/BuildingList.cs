using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Buildings", menuName = "Scriptable/BuildingList",order = 0)]
public class BuildingList : ScriptableObject
{
    public List<BuildingInfo> Buildings = new List<BuildingInfo>();

    
}

[System.Serializable]
public class BuildingInfo
{
    public string Name;
    public string Code;
    public string UpgradeFrom;
    public int Cost;
    public int UpgradeCost;
    public TechTier Tier;
    public GameObject Building;

    public bool CanUpgrade;
    public bool CanBuild;
}