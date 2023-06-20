using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingInfo
{
    public float Cost;
    public Vector3 WorldPostion;
    public GameObject LinkedObject;

    public PathfindingInfo()
    {
        Cost = 0;
    }

}
