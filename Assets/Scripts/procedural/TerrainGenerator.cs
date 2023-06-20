using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoService<TerrainGenerator>
{

    public Dictionary<string, TerrainData> TerrainStorage = new Dictionary<string, TerrainData>();

    public GameObject TerrainBlock;

    private void Awake()
    {
        RegisterService();
    }


    public void OnDestroy()
    {
        UnregisterService();
    }


    void GenerateTerrain(Vector3 postion,Vector2Int MapSize)
    {
        TerrainData Data = new TerrainData();
        Data.RootObject = Instantiate<GameObject>(new GameObject("Root"),postion,Quaternion.identity);

        var Perlin = Services.Resolve<PerlinGenerator>();

        var BaseTerrain = Perlin.GenerateNoiseMap(MapSize);
        var River = Perlin.GenerateNoiseMap(MapSize);
        var Hills = Perlin.GenerateNoiseMap(MapSize);

        //for (int i = 0; i < BaseTerrain.GetLength(0); i++)
        //{
        //    for (int b = 0; b < BaseTerrain.GetLength(1); b++)
        //    {
        //        if(River[i,b] < 0.3f && River[i, b] < BaseTerrain[i,b])
        //        {
        //            BaseTerrain[i, b] = River[i, b];
        //        }
        //    }
        //}

        var GridService = Services.Resolve<GridController>();
        GridService.CreateGrid<PathfindingInfo>(MapSize, "Battle");

        var GridArray = GridService.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");



        for (int i = 0; i < MapSize.x; i++)
        {
            for (int b = 0; b < MapSize.y; b++)
            {
                
                var A = Instantiate(TerrainBlock, new Vector3(i, BaseTerrain[i, b] * 5f, b), Quaternion.identity,Data.RootObject.transform);

                GridArray[i, b].Contents.WorldPostion = A.transform.position;
                GridArray[i, b].Contents.Cost = 1;
                GridArray[i, b].Contents.LinkedObject = A;
                A.GetComponent<Renderer>().material.color = TerrainColor(BaseTerrain[i, b]);
                Data.TerrainBlocks.Add(A);
                A.name = string.Format("Block: X {0} | y{1}", i, b);
            }
        }

        GridArray[50, 50].Contents.LinkedObject.GetComponent<Renderer>().material.color = Color.red;

    }

    IEnumerator AnimatedGenerateTerrain(Vector3 postion, Vector2Int MapSize)
    {

        TerrainData Data = new TerrainData();
        Data.RootObject = Instantiate<GameObject>(new GameObject("Root"), postion, Quaternion.identity);

        var Perlin = Services.Resolve<PerlinGenerator>();

        var BaseTerrain = Perlin.GenerateNoiseMap(MapSize);
        var River = Perlin.GenerateNoiseMap(MapSize);
        var Hills = Perlin.GenerateNoiseMap(MapSize);

        int Skipcalulated= MapSize.x + MapSize.y / 3;
        int SkipCounter = 0;

        for (int i = 0; i < MapSize.x; i++)
        {
            for (int b = 0; b < MapSize.y; b++)
            {
                SkipCounter++;
                if(SkipCounter > Skipcalulated)
                {
                    yield return new WaitForEndOfFrame();
                    SkipCounter = 0;
                }
                var A = Instantiate(TerrainBlock, new Vector3(i, BaseTerrain[i, b] * 5f, b), Quaternion.identity, Data.RootObject.transform);
                A.GetComponent<Renderer>().material.color = TerrainColor(BaseTerrain[i, b]);
                Data.TerrainBlocks.Add(A);

                A.name = string.Format("Block: X {0} | y{1}", i, b);
            }
        }
        
    }

    Color TerrainColor(float H)
    {

        //if(H > 0.9f)
        //{
        //    return new Color(Mathf.Clamp(H, 0.9f, 1f), Mathf.Clamp(H, 0.9f, 1f), Mathf.Clamp(H, 0.9f, 1f));
        //}

        if(H > 0.2f)
        {
            return new Color(0, Mathf.Clamp(H, 0.2f, 1f), 0);
        }
        else
        {
            return new Color(0, 0, Mathf.Clamp(H, 0.5f, 1f));
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain(Vector3.zero, new Vector2Int(100, 100));
        //StartCoroutine(TestFlood());


        Services.Resolve<FlowField>().GenerateFlowField();

        //StartCoroutine(AnimatedGenerateTerrain(Vector3.zero, new Vector2Int(100, 100)));
    }


    IEnumerator TestFlood()
    {
        var GC = Services.Resolve<GridController>();

        var GridArray = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

        //Queue<GridCell<PathfindingInfo>> RedTest = new Queue<GridCell<PathfindingInfo>>();

        List<GridCell<PathfindingInfo>> Search = new List<GridCell<PathfindingInfo>>();
        List<GridCell<PathfindingInfo>> Done = new List<GridCell<PathfindingInfo>>();
        Search.Add(GridArray[50, 50]);
        Search[0].Contents.Cost = 0;
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
                        Target.NeighboursList[i].Contents.Cost = Target.Contents.Cost+1;
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

}
