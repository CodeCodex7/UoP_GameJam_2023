using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell<T>
{

    public T Contents;
    public List<GridCell<T>> NeighboursList = new List<GridCell<T>>();
    public List<GridCell<T>> DirectionalNeighboursList = new List<GridCell<T>>();

    public GridCell(T input)
    {
        Contents = input;
    }

    void Populateneighbours(params GridCell<T> [] gridCells )
    {
        for (int i = 0; i < gridCells.Length; i++)
        {
            NeighboursList.Add(gridCells[i]);
        }
    }


}



