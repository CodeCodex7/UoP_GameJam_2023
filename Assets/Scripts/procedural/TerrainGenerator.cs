using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using System;
public class TerrainGenerator : MonoService<TerrainGenerator>
{

    public Dictionary<string, TerrainData> TerrainStorage = new Dictionary<string, TerrainData>();

    public GameObject TerrainBlock;
    public GameObject TestAI;

    TerrainData Data;

    private void Awake()
    {
        RegisterService();
    }


    public void OnDestroy()
    {
        UnregisterService();
    }


    public void GenerateTerrain(Vector3 postion,Vector2Int MapSize)
    {
        Data = new TerrainData();
        Data.RootObject = Instantiate<GameObject>(new GameObject("Root"),postion,Quaternion.identity);

        var Perlin = Services.Resolve<PerlinGenerator>();

        var BaseTerrain = Perlin.GenerateNoiseMap(MapSize);
        var River = Perlin.GenerateNoiseMap(MapSize);
        
        var GridService = Services.Resolve<GridController>();
        
        GridService.CreateGrid<PathfindingInfo>(MapSize, "Battle");
        GridService.CreateGrid<UnitMapData>(MapSize, "BattleInfo");

        var GridArray = GridService.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");
        var BattleArea = GridService.GetFromStorage<GridCell<PathfindingInfo>[,]>("BattleInfo");
        
        //Debug.Log(BattleArea[0, 0].Contents.Occuipied);

        for (int i = 0; i < MapSize.x; i++)
        {
            for (int b = 0; b < MapSize.y; b++)
            {
                
                var A = Instantiate(TerrainBlock, new Vector3(i, BaseTerrain[i, b] * 5f, b), Quaternion.identity,Data.RootObject.transform);

                //A.GetComponentInChildren<TextMeshPro>().text = string.Format("{0} {1}", i, b);

                GridArray[i, b].Contents.GridPostion = new Vector2Int(i, b);
                GridArray[i, b].Contents.WorldPostion = A.transform.position;

                if(BaseTerrain[i, b] <0.2f)
                {
                    GridArray[i, b].Contents.Cost =255;
                }
                else if(BaseTerrain[i, b] > 0.5f)
                {
                    GridArray[i, b].Contents.Cost = 3;
                }
                else
                {
                    GridArray[i, b].Contents.Cost = 1;
                }
                GridArray[i, b].Contents.LinkedObject = A;
                A.GetComponent<Renderer>().material.color = TerrainColor(BaseTerrain[i, b]); //TerrainCost(GridArray[i, b].Contents.Cost);//TerrainColor(BaseTerrain[i, b]);
                Data.TerrainBlocks.Add(A);
                A.name = string.Format("Block: X {0} | y{1}", i, b);
            }
        }

        //StartCoroutine(AnimatedGenerateTerrain());

    }

    public void DestoryTerrain()
    {
        Destroy(Data.RootObject);
    }

    Color TerrainColor(float H)
    {

        //if(H > 0.9f)
        //{
        //    return new Color(Mathf.Clamp(H, 0.9f, 1f), Mathf.Clamp(H, 0.9f, 1f), Mathf.Clamp(H, 0.9f, 1f));
        //}

        if(H > 0.2f)
        {
            return new Color(Mathf.Clamp(H, 0.2f, 1f), Mathf.Clamp(H, 0.2f, 1f), 0);
            //return new Color(0, Mathf.Clamp(H, 0.2f, 1f), 0);
        }
        else
        {
            return new Color(0, 0, Mathf.Clamp(H, 0.5f, 1f));
        }

    }

    Color TerrainCost(int H)
    {
        switch (H)
        {
            case 1:
                return Color.green;

            case 3:
                return Color.white;

            case 255:
                return Color.blue;

                default: return Color.black;

        }
    }
  
    void Start()
    {
        //GenerateTerrain(Vector3.zero, new Vector2Int(Services.Resolve<GameManager>().MapSize.x, Services.Resolve<GameManager>().MapSize.y));

        //Services.Resolve<BattleController>().Battle(20,20);
        
    }



    IEnumerator TestFlood()
    {
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        //Queue<GridCell<PathfindingInfo>> RedTest = new Queue<GridCell<PathfindingInfo>>();

        List<GridCell<PathfindingInfo>> Search = new List<GridCell<PathfindingInfo>>();
        List<GridCell<PathfindingInfo>> Done = new List<GridCell<PathfindingInfo>>();
        Search.Add(GridArray[50, 50]);
        Search[0].Contents.Weight = 0;
        yield return null;


        int counter=0;
        while (Search.Count != 0)
        {
            counter++;
            if(counter > 100)
            {

                yield return null;
                counter = 0;
            }


            GridCell<PathfindingInfo> Target = Search[0];
            Search.RemoveAt(0);
         
            if (!Done.Contains(Target))
            {
                Done.Add(Target);
            }

            for (int i = 0; i < Target.NeighboursList.Count; i++)
            {
                if (!Done.Contains(Target.NeighboursList[i]))
                {
                    if (!Search.Contains(Target.NeighboursList[i]))
                    {
                        Target.NeighboursList[i].Contents.Weight = Target.Contents.Weight+1;
                        Search.Add(Target.NeighboursList[i]);
                    }
                }
            }

        }
        Debug.Log("Flood Test Completed");
    }

    // Update is called once per frame
    void Update()
    {

    }

 
    public class TerrainData
    {
        public string Name;
        public GameObject RootObject;
        public Vector3 Postion;

        public List<GameObject> TerrainBlocks = new List<GameObject>();

    }

    public class MapData
    {
        public BasicUnit Unit;
        public bool Occuipied;

        public MapData( ) 
        {
            Occuipied = false;
        }
    }

}
