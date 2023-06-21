using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


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
    public void GenerateFlowField(Vector2Int EndPostion)
    {

        var watch = System.Diagnostics.Stopwatch.StartNew();
        // the code that you want to measure comes here
        
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        Queue<GridCell<PathfindingInfo>> Open = new Queue<GridCell<PathfindingInfo>>();
        HashSet<GridCell<PathfindingInfo>> Closed = new HashSet<GridCell<PathfindingInfo>>();

        GridArray[EndPostion.x, EndPostion.y].Contents.Weight = 0;
        GridArray[EndPostion.x, EndPostion.y].Contents.LinkedObject.GetComponent<Renderer>().material.color = Color.red;
        Open.Enqueue(GridArray[EndPostion.x, EndPostion.y]);
        
        float HCost = 0;
        GridCell<PathfindingInfo> Target;

        foreach (var cell in GridArray)
        {
            cell.Contents.Weight = 1;
        }

        while (Open.Count != 0)
        {

            
            Target = Open.Dequeue();


            //for (int i = 0; i < Target.NeighboursList.Count; i++)
            //{
            //    if (Target.NeighboursList[i].Contents.Cost > HCost)
            //    {
            //        HCost = Target.NeighboursList[i].Contents.Cost;
            //    }
            //}

            //Target.Contents.Cost = HCost++;

            //Target.Contents.LinkedObject.name = string.Format("{0} Cost is {1}", Target.Contents.LinkedObject.name, Target.Contents.Cost);
            Target.Contents.LinkedObject.GetComponentInChildren<TextMeshPro>().text = Target.Contents.Weight.ToString();

            for (int i = 0; i < Target.NeighboursList.Count; i++)
            {
                if (!Closed.Contains(Target.NeighboursList[i]))
                {
                    if (!Open.Contains(Target.NeighboursList[i]))
                    {
                        Target.NeighboursList[i].Contents.Weight = Target.Contents.Weight + 1;
                        Open.Enqueue(Target.NeighboursList[i]);
                        continue;
                    }                   ;
                }
            }


            Closed.Add(Target);
            HCost = 0;
        }
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log(watch.Elapsed);



    }

    public void GenerateCostField(Vector2Int StartPos)
    {
   
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        Queue<GridCell<PathfindingInfo>> Open = new Queue<GridCell<PathfindingInfo>>();
        HashSet<GridCell<PathfindingInfo>> Closed = new HashSet<GridCell<PathfindingInfo>>();

        GridArray[StartPos.x, StartPos.y].Contents.BestCost = 0;
        GridArray[StartPos.x, StartPos.y].Contents.LinkedObject.GetComponent<Renderer>().material.color = Color.red;
        Open.Enqueue(GridArray[StartPos.x, StartPos.y]);

        float HCost = 0;
        GridCell<PathfindingInfo> Target;

        while (Open.Count != 0)
        {

            Target = Open.Dequeue();
            Target.Contents.CostIncresse(1);
            //Target.Contents.LinkedObject.name = string.Format("{0} Cost is {1}", Target.Contents.LinkedObject.name, Target.Contents.Cost);
            Target.Contents.LinkedObject.GetComponentInChildren<TextMeshPro>().text = Target.Contents.Cost.ToString();
            //Target.Contents.LinkedObject.GetComponent<Renderer>().material.color = Color.red;

            for (int i = 0; i < Target.DirectionalNeighboursList.Count; i++)
            {
                if (!Closed.Contains(Target.DirectionalNeighboursList[i]))
                {
                    if (!Open.Contains(Target.DirectionalNeighboursList[i]))
                    {                      
                        //Target.DirectionalNeighboursList[i].Contents.CostIncresse(1);
                        Open.Enqueue(Target.DirectionalNeighboursList[i]);
                    };
                }
            }

            Closed.Add(Target);
            HCost = 0;
        }
    }

    public void GenerateIntergationField(Vector2Int StartPos)
    {

        var GC = Services.Resolve<GridController>();
        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        GridCell<PathfindingInfo> Target = GridArray[StartPos.x,StartPos.y];

        Target.Contents.Cost = 0;
        Target.Contents.BestCost = 0;


        Queue<GridCell<PathfindingInfo>> CellsToCheck = new Queue<GridCell<PathfindingInfo>>();
        CellsToCheck.Enqueue(Target);

        GridCell<PathfindingInfo> Current;

        while (CellsToCheck.Count > 0)
        {
            Current = CellsToCheck.Dequeue();
            
            foreach (GridCell<PathfindingInfo> grid in Current.DirectionalNeighboursList)
            {
                if (grid.Contents.Cost == 255) { continue; }
                if(grid.Contents.Cost + Current.Contents.BestCost < grid.Contents.BestCost)
                {
                    grid.Contents.BestCost = (Current.Contents.Cost+ Current.Contents.BestCost);
                    CellsToCheck.Enqueue (grid);                  
                }
            }
        }

        UpdateText(Vector2Int.zero);
    }

    public void UpdateText(Vector2Int StartPos)
    {

        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        Queue<GridCell<PathfindingInfo>> Open = new Queue<GridCell<PathfindingInfo>>();
        HashSet<GridCell<PathfindingInfo>> Closed = new HashSet<GridCell<PathfindingInfo>>();

        Open.Enqueue(GridArray[StartPos.x, StartPos.y]);

        GridCell<PathfindingInfo> Target;

        while (Open.Count != 0)
        {

            Target = Open.Dequeue();          
            Target.Contents.LinkedObject.GetComponentInChildren<TextMeshPro>().text = Target.Contents.BestCost.ToString();
            //Target.Contents.LinkedObject.GetComponent<Renderer>().material.color = Color.red;

            for (int i = 0; i < Target.DirectionalNeighboursList.Count; i++)
            {
                if (!Closed.Contains(Target.DirectionalNeighboursList[i]))
                {
                    if (!Open.Contains(Target.DirectionalNeighboursList[i]))
                    {                      
                        Open.Enqueue(Target.DirectionalNeighboursList[i]);
                    };
                }
            }

            Closed.Add(Target);        
        }
    }

    public void ResetBest()
    {
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        foreach (var item in GridArray)
        {
            item.Contents.BestCost = 255;
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
