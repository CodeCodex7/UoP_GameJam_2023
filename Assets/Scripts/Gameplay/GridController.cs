using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;




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

    public T GetFromStorage<T>(string Key) where T : class
    {
        return GridStorage[Key] as T;
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


    void Test()
    {
        CreateGrid<PathfindingInfo>(new Vector2Int(5, 5), "PathGrid");

        Debug.Log(GetFromStorage<GridCell<PathfindingInfo>[,]>("PathGrid")[2, 2].Contents.Weight);

    }





}
