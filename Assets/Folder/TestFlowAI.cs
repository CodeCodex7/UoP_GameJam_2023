using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlowAI : MonoBehaviour
{

    public float Speed = 0.5f;
    Vector2Int PosTraker;
    GridCell<PathfindingInfo> Cell;

    public Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position != TargetPos) 
        {
           this.transform.position =Vector3.MoveTowards(this.transform.position, TargetPos, Speed * Time.deltaTime);
        }
    }
   
    //public IEnumerator FreeSeek(string Key, Vector2Int StartPos)
    //{
    //    var GC = Services.Resolve<GridController>();

    //    var Grid = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>(Key);
    //    var BattleInfo = GC.GetFromStorage<GridCell<MapData>[,]>("BattleInfo");

    //    this.transform.position = Grid[StartPos.x, StartPos.y].Contents.LinkedObject.gameObject.transform.position;
    //    PosTraker.Set(StartPos.x, StartPos.y);
    //    TargetPos = Grid[StartPos.x, StartPos.y].Contents.LinkedObject.transform.position;
    //    BattleInfo[PosTraker.x, PosTraker.y].Contents.Occuipied = true;
    //   // BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = this;
    //    PathfindingInfo Target = Grid[StartPos.x, StartPos.y].Contents;
    //    float Cost = Grid[StartPos.x, StartPos.y].Contents.Weight;

    //    Speed = Random.Range(Speed, Speed + 2);
    //    while (true)
    //    {
    //        Cost = Grid[PosTraker.x, PosTraker.y].Contents.BestCost;

    //        yield return null;

    //        for (int i = 0; i < Grid[PosTraker.x, PosTraker.y].NeighboursList.Count; i++)
    //        {
    //            if (Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost < Cost && BattleInfo[PosTraker.x, PosTraker.y].NeighboursList[i].Contents == false)
    //            {
    //                if (this.transform.position == TargetPos)
    //                {
    //                    Cost = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost;
    //                    Target = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents;
    //                }
    //            }
                
    //        }

    //        BattleInfo[PosTraker.x, PosTraker.y].Contents = false;
    //        PosTraker = Target.GridPostion;            
    //      //  this.transform.position = Target.LinkedObject .transform.position;
    //        TargetPos = Target.LinkedObject.transform.position;
    //        BattleInfo[PosTraker.x, PosTraker.y].Contents = true;
    //    }               
    //}

}
