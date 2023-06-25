using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class GridController : MonoService<GridController>
{

    private Dictionary<string, object> GridStorage = new Dictionary<string, object>();

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }


    private void Start()
    {

    }

    public bool GridExists(string Key)
    {
        if(GridStorage.ContainsKey(Key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public T GetFromStorage<T>(string Key) where T : class
    {
        return GridStorage[Key] as T;
    }


    public void CopyGrid<T>(string Key,string NewKey) where T : new()
    {

        var P = GetFromStorage<GridCell<T>[,]>(Key);

        GridCell<T>[,] cells = new GridCell<T>[P.GetLength(0), P.GetLength(1)];



        for (int i = 0; i < P.GetLength(0); i++)
        {
            for (int b = 0; b < P.GetLength(0); b++)
            {
                cells[i, b] = new GridCell<T>(new T());
            }
        }

        for (int i = 0; i < P.GetLength(0); i++)
        {
            for (int b = 0; b < P.GetLength(0); b++)
            {
                cells[i, b] = P[i, b]; ;
            }
        }



        GridStorage.Add(NewKey, cells);
    }

    public void CreateGrid<T>(Vector2Int Size, string Key) where T : new()
    {

        GridCell<T>[,] cells = new GridCell<T>[Size.x, Size.y];

        for (int i = 0; i < Size.x; i++)
        {
            for (int b = 0; b < Size.y; b++)
            {
                cells[i, b] = new GridCell<T>(new T());
            }
        }


        LinkCells<T>(cells);
        GridStorage.Add(Key, cells);

    }

    public void LinkCells<T>(GridCell<T>[,] Cells)
    {

        for (int i = 0; i < Cells.GetLength(0); i++)
        {
            for (int b = 0; b < Cells.GetLength(1); b++)
            {

                if (!((i + 1) > Cells.GetLength(0) - 1))
                    Cells[i, b].NeighboursList.Add(Cells[i + 1, b]);

                if (!((i - 1) < 0))
                    Cells[i, b].NeighboursList.Add(Cells[i - 1, b]);

                if (!((i + 1) > Cells.GetLength(0) - 1) && !((b + 1) > Cells.GetLength(1) - 1))
                    Cells[i, b].NeighboursList.Add(Cells[i + 1, b + 1]);

                if (!((i + 1) > Cells.GetLength(0) - 1) && !((b - 1) < 0))
                    Cells[i, b].NeighboursList.Add(Cells[i + 1, b - 1]);

                if (!((i - 1) < 0) && !((b - 1) < 0))
                    Cells[i, b].NeighboursList.Add(Cells[i - 1, b - 1]);

                if (!((i - 1) < 0) && !((b + 1) > Cells.GetLength(1) - 1))
                    Cells[i, b].NeighboursList.Add(Cells[i - 1, b + 1]);

                if (!((b - 1) < 0))
                    Cells[i, b].NeighboursList.Add(Cells[i, b - 1]);

                if (!((b + 1) > Cells.GetLength(1) - 1))
                    Cells[i, b].NeighboursList.Add(Cells[i, b + 1]);

            }
        }

        for (int i = 0; i < Cells.GetLength(0); i++)
        {
            for (int b = 0; b < Cells.GetLength(1); b++)
            {

                if (!((i + 1) > Cells.GetLength(0) - 1))
                    Cells[i, b].DirectionalNeighboursList.Add(Cells[i + 1, b]);

                if (!((i - 1) < 0))
                    Cells[i, b].DirectionalNeighboursList.Add(Cells[i - 1, b]);

                if (!((b - 1) < 0))
                    Cells[i, b].DirectionalNeighboursList.Add(Cells[i, b - 1]);

                if (!((b + 1) > Cells.GetLength(1) - 1))
                    Cells[i, b].DirectionalNeighboursList.Add(Cells[i, b + 1]);

            }
        }

    }

    public void Clear()
    {
        //foreach(KeyValuePair<String,object> valuePair in GridStorage)
        //{
        //    valuePair.;
        //}
        GridStorage.Clear();
    }

    void Test()
    {
        CreateGrid<PathfindingInfo>(new Vector2Int(5, 5), "PathGrid");

        Debug.Log(GetFromStorage<GridCell<PathfindingInfo>[,]>("PathGrid")[2, 2].Contents.Weight);

    }





}
