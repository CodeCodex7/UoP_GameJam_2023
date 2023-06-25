using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingInfo
{
    public int Weight;
    public int Cost;
    public int BestCost;

    public Vector3 WorldPostion;
    public Vector2Int GridPostion;
    public GameObject LinkedObject;

    public PathfindingInfo()
    {
        Weight = 1;
        Cost = 0;
        BestCost = 255;
    }

    public void CostIncresse(int Ammount)
    {
        if(Cost == 255) { return; }
        if(Ammount + Cost >= 255) { Cost = 255; }
        else { Cost += Ammount; }


    }

}
