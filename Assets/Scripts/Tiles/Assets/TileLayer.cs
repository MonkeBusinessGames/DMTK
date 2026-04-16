using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileLayer
{
    public string layerName;
    public int[] tiles;
    public bool hide;

    public TileLayer()
    {
        tiles = new int[0];
    }


    public TileLayer(string newName, int[] tileList, bool isHidden)
    {
        layerName = newName;
        tiles = new int[tileList.Length];
        hide = isHidden;
    }
    
    public override string ToString()
    {
        return layerName + ", " + hide + ", " + tiles;
    }
}
