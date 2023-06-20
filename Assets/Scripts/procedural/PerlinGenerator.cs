using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGenerator : MonoService<PerlinGenerator>
{

    public GameObject TestObject;

    public float AreaZoom = 0.05f;
    
    private void Awake()
    {
        RegisterService();
    }

    
    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    
    public float[,] GenerateNoiseMap(Vector2Int MapSize)
    {
        float SampleOrigin = UnityEngine.Random.Range(0f, 10000f);

        float[,] PerlinGrid = new float[MapSize.x , MapSize.y];
        for (int i = 0; i < MapSize.x; i++)
        {
            for (int b = 0; b < MapSize.y; b++)
            {
               PerlinGrid[i,b] = Mathf.PerlinNoise(SampleOrigin + (0.05f *i),SampleOrigin + (0.05f * b));
            }
        }

        return PerlinGrid;
    }

    void Test()
    {

        var grid = GenerateNoiseMap(new Vector2Int(100, 100));


        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int b = 0; b < grid.GetLength(0) ; b++)
            {
               var A = Instantiate(TestObject, new Vector3(i, grid[i, b], b),Quaternion.identity);
                A.GetComponent<Renderer>().material.color = new Color(0,Mathf.Clamp(grid[i, b],0.4f,1f), 0);
            }
        }


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
