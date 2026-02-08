using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridMap
{
    public string mapName;
    public int width, height;
    public GridTile[][,] tileLayers;
    public List<GridObject> objects;


    public GridMap() 
    {
    }

    public GridMap(string name, int gridWidth, int gridHeight)
    {
        mapName = name;
        width = gridWidth;
        height = gridHeight;
    }
}
