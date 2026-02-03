using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridMap
{
    public Vector2 size;
    public string parent;
    public GridTile[][,] tileLayers;
    public List<GridObject> objects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
