using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileLayer
{
    public string layerName;
    public int[] tiles;
    public bool hide;

    /// <summary>
    /// Create an empty tile layer
    /// </summary>
    public TileLayer()
    {
        layerName = "New";
        tiles = new int[0];
        hide = false;
    }

    ///Create a tile layer with known tiles
    public TileLayer(string newName, int[] tileList, bool isHidden)
    {
        layerName = newName;
        tiles = tileList;
        hide = isHidden;
    }

    public override string ToString()
    {
        return layerName + ", " + hide + ", " + string.Join(", ", tiles);
    }
}
