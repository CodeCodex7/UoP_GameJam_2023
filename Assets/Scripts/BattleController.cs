using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class BattleController : MonoService<BattleController> 
{

    public List<GameObject> Units;

    public List<BasicUnit> UnitList = new List<BasicUnit>();

    public List<BasicUnit> DeadUnits = new List<BasicUnit>();


    public void GameOver()
    {

        if (CountTeamAlive(1) == 0)
        {
            Services.Resolve<UiController>().ActiveUI("GameOver");
            Services.Resolve<UiController>().ActiveUI("PlayerWin");
            
        }

        if (CountTeamAlive(0) == 0)
        {
            Services.Resolve<UiController>().ActiveUI("GameOver");
            Services.Resolve<UiController>().ActiveUI("AIWin");
        }

    }

    public void SetupBattle(List<UnitTypes> units)
    {
        var G = Services.Resolve<GameManager>();
        Services.Resolve<FreeRoamCam>().FreeRoam = true;
        Services.Resolve<TerrainGenerator>().GenerateTerrain(Vector3.zero, G.MapSize);
        
        SpawnUnits(units.Count, units.Count, units);
    }
    public void Clear()
    {

        for (int i = 0; i < DeadUnits.Count; i++)
        {
            Destroy(DeadUnits[i].gameObject);
        }

        for (int i = 0; i < UnitList.Count; i++)
        {
            Destroy(UnitList[i].gameObject);
        }

        UnitList.Clear();
        DeadUnits.Clear();
    }

    public int CountTeamAlive(int team)
    {
        int Count =0;
        foreach (var item in UnitList)
        {
            if(!item.Dead && item.Team == team)
            {
                Count++;
            }
        }
        return Count;
    }

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    public void UnitDeath(BasicUnit unit)
    {
        Debug.Log(string.Format("{0} of Team {1} Died", unit.gameObject.name, unit.Team));
        DeadUnits.Add(unit);
        
        //Check GameOver
        GameOver();
    }

    public void Battle(int BlueUnits,int RedUnits)
    {
        SpawnUnits(BlueUnits,RedUnits);
    }

    void SpawnUnits(int R,int B)
    {
        List<Vector2Int> RedSpawns =new List<Vector2Int>();
        List<Vector2Int> BlueSpawns = new List<Vector2Int>();

        var BattleInfo = Services.Resolve<GridController>().GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        for (int i = 0; i < R; i++)
        {
            try
            {

                Vector2Int V;
                while (true)
                {
                    V = new Vector2Int(UnityEngine.Random.Range(0, Services.Resolve<GameManager>().MapSize.x/2), UnityEngine.Random.Range(0, Services.Resolve<GameManager>().MapSize.y));
                    if(BattleInfo[V.x,V.y].Contents.Cost !=255)
                    {
                        break;
                    }
                }

              RedSpawns.Add(V);
            }
            catch { }

        }

        for (int i = 0; i < B; i++)
        {
            Vector2Int V;
            while (true)
            {
                V =  new Vector2Int(UnityEngine.Random.Range(Services.Resolve<GameManager>().MapSize.x/2, Services.Resolve<GameManager>().MapSize.y), UnityEngine.Random.Range(0, Services.Resolve<GameManager>().MapSize.y));
                if (BattleInfo[V.x, V.y].Contents.Cost != 255)
                {
                    break;
                }
            }
            BlueSpawns.Add(V);
        }


        foreach (var item in RedSpawns)
        {
            SpawnUnit(0,item, UnitList.Count);
        }

        foreach (var item in BlueSpawns)
        {
            SpawnUnit(1, item, UnitList.Count);
        }
    }

    void SpawnUnits(int R, int B,List<UnitTypes> Ulist)
    {
        List<Vector2Int> RedSpawns = new List<Vector2Int>();
        List<Vector2Int> BlueSpawns = new List<Vector2Int>();

        var BattleInfo = Services.Resolve<GridController>().GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        int Quater = Services.Resolve<GameManager>().MapSize.x / 4;

        for (int i = 0; i < R; i++)
        {
            try
            {

                Vector2Int V;
                while (true)
                {
                    V = new Vector2Int(UnityEngine.Random.Range(0, Quater), UnityEngine.Random.Range(0, Services.Resolve<GameManager>().MapSize.y));
                    if (BattleInfo[V.x, V.y].Contents.Cost != 255)
                    {
                        break;
                    }
                }

                RedSpawns.Add(V);
            }
            catch { }

        }

        for (int i = 0; i < B; i++)
        {
            Vector2Int V;
            while (true)
            {
                V = new Vector2Int(UnityEngine.Random.Range(Services.Resolve<GameManager>().MapSize.x / 4 *3, Services.Resolve<GameManager>().MapSize.y), UnityEngine.Random.Range(0, Services.Resolve<GameManager>().MapSize.y));
                if (BattleInfo[V.x, V.y].Contents.Cost != 255)
                {
                    break;
                }
            }
            BlueSpawns.Add(V);
        }


        for (int i = 0; i < Ulist.Count; i++)
        {
            SpawnUnit(0, RedSpawns[i], UnitList.Count, Ulist[i]);
        }

        foreach (var item in BlueSpawns)
        {
            SpawnUnit(1, item, UnitList.Count);
        }
    }

    void SpawnUnit(int team,Vector2Int loc,int ID)
    {
        var AI = Instantiate(Units[UnityEngine.Random.Range(0,Units.Count)]);
        


        AI.name = string.Format("Agent {0} of Team {1}", ID,team);
        AI.GetComponent<BasicAI>().PosTraker = loc;
        AI.GetComponent<BasicAI>().MapKey = string.Format("Map{0}", ID);
        AI.GetComponent<BasicUnit>().Team = team;

        var GC = Services.Resolve<GridController>();
        var BattleGrid = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

        var Battle = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        AI.transform.position = Battle[loc.x, loc.y].Contents.LinkedObject.transform.position;

        BattleGrid[loc.x, loc.y].Contents.Unit = AI.GetComponent<BasicUnit>();

        BasicUnit BU = AI.GetComponent<BasicUnit>();
        UnitList.Add(AI.GetComponent<BasicUnit>());

       var R = AI.GetComponentsInChildren<Renderer>();
        foreach (var r in R)
        {
            switch (team)
            {
                case 0:
                    { r.material.color = Color.red; };
                    break;

                case 1:
                    { r.material.color = Color.blue; };
                    break;

                default:
                    break;
            }
           
        }
       
    }

    void SpawnUnit(int team, Vector2Int loc, int ID,UnitTypes unit)
    {
        var AI = Instantiate(Units[(int)unit]);

        AI.name = string.Format("Agent {0} of Team {1}", ID, team);
        AI.GetComponent<BasicAI>().PosTraker = loc;
        AI.GetComponent<BasicAI>().MapKey = string.Format("Map{0}", ID);
        AI.GetComponent<BasicUnit>().Team = team;

        var GC = Services.Resolve<GridController>();
        var BattleGrid = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

        var Battle = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        AI.transform.position = Battle[loc.x, loc.y].Contents.LinkedObject.transform.position;

        BattleGrid[loc.x, loc.y].Contents.Unit = AI.GetComponent<BasicUnit>();

        BasicUnit BU = AI.GetComponent<BasicUnit>();
        UnitList.Add(AI.GetComponent<BasicUnit>());

        var R = AI.GetComponentsInChildren<Renderer>();
        foreach (var r in R)
        {
            switch (team)
            {
                case 0:
                    { r.material.color = Color.red; };
                    break;

                case 1:
                    { r.material.color = Color.blue; };
                    break;

                default:
                    break;
            }

        }

    }


}

