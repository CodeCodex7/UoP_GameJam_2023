using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class FlowField : MonoService<FlowField>
{


    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }
    public void GenerateFlowField()
    {
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        List<GridCell<PathfindingInfo>> Search = new List<GridCell<PathfindingInfo>>();
        List<GridCell<PathfindingInfo>> Done = new List<GridCell<PathfindingInfo>>();

        Search.Add(GridArray[50, 50]);
        Search[0].Contents.Cost = 0;

        while (Search.Count != 0)
        {

            GridCell<PathfindingInfo> Target = Search[0];
            Search.RemoveAt(0);

            float HCost = 0;

            for (int i = 0; i < Target.NeighboursList.Count; i++)
            {
                if (Target.NeighboursList[i].Contents.Cost < HCost)
                {
                    HCost = Target.NeighboursList[i].Contents.Cost;
                }
            }

            Target.Contents.Cost = HCost + 1;
            Target.Contents.LinkedObject.name = string.Format("{0} Cost is {1}", Target.Contents.LinkedObject.name, Target.Contents.Cost);

            {
                Done.Add(Target);
            }

            for (int i = 0; i < Target.NeighboursList.Count; i++)
            {
                if (!Done.Contains(Target.NeighboursList[i]))
                {
                    if (!Search.Contains(Target.NeighboursList[i]))
                    {
                        
                        Search.Add(Target.NeighboursList[i]);
                    }
                }
            }

        }

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
