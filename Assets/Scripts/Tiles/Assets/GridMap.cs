using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridMap
{
    public string mapName;
    public int width, height;
    public List<TileLayer> tileLayers;
    public List<GridObject> objects;


    public GridMap() 
    {
    }

    public GridMap(string name, int gridWidth, int gridHeight)
    {
        mapName = name;
        width = gridWidth;
        height = gridHeight;

        tileLayers = new List<TileLayer> ();
    }

    public override string ToString()
    {
        return mapName + ", " + width + ", " + height;
    }

    public void AddLayer(string name)
    {
        tileLayers.Add(new TileLayer(name, new int[width * height], false));
    }

    public void RemoveLayer(int index) 
    {
        if(index >= 0 && index < tileLayers.Count)
        {
            tileLayers.RemoveAt(index);
        }
    }

    public void MoveLayerUp(int index)
    {
        if (index > 0 && index < tileLayers.Count)
        {
            TileLayer temp = tileLayers[index];
            tileLayers.RemoveAt(index);
            tileLayers.Insert(index - 1, temp);

        }
    }

    public void MoveLayerDown(int index)
    {
        if (index >= 0 && index < tileLayers.Count - 1)
        {
            TileLayer temp = tileLayers[index];
            tileLayers.RemoveAt(index);
            tileLayers.Insert(index + 1, temp);

        }
    }

}
