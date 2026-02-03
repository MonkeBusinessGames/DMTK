using UnityEngine;

[System.Serializable]
public class TileRules
{
    public string ruleName;
    public string parent;
    public string defaultTile;
    public string[] edgeTiles; // N, E, S, W
    public string[] cornerTiles;// NE, SE, SW, NW


}
