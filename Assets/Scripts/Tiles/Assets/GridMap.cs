using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

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
        tileLayers.Add(new TileLayer("Default Layer", new int[gridWidth *  gridHeight], false));
    }

    public override string ToString()
    {
        return mapName + ", " + width + ", " + height;
    }

    public void AddLayer(string name)
    {
        tileLayers.Add(new TileLayer(name, new int[width * height], false));
    }


    public void UpdateLayerName(string name, int index)
    {
        tileLayers[index].layerName = name;
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

    public void HideLayer(int index)
    {
        tileLayers[index].hide = true;
    }

    public void ShowLayer(int index)
    {
        tileLayers[index].hide = false;
    }

    public void UpdateSize(int newWidth, int newHeight)
    {
        UnityEngine.Debug.Log("Size Changed from " + width + ", " + height + " to " + newWidth + ", " + newHeight);

        int wOffset = (newWidth - width) / 2;
        int hOffset = (newHeight - height) / 2;

        UnityEngine.Debug.Log("wOffset =  " + wOffset + " & hOffset = " + hOffset);


        foreach (TileLayer layer in tileLayers)
        {
            UnityEngine.Debug.Log("Old Tile Layer " + layer);

            int[] temp = new int[newWidth  * newHeight];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int oldIndex = x + y * width;

                    if (layer.tiles[oldIndex] == 0)
                        continue;

                    int newX = x + wOffset;
                    int newY = y + hOffset;

                    UnityEngine.Debug.Log("newX =  " + newX + " & newY = " + newY);

                    if (newX < 0 || newX >= newWidth)
                        continue;

                    if (newY < 0 || newY >= newHeight)
                        continue;

                    int newIndex = newX + newY * newWidth;

                    temp[newIndex] = layer.tiles[oldIndex];

                    UnityEngine.Debug.Log("Added back sprite " + layer.tiles[oldIndex] + " from index " + oldIndex + " to index " + newIndex);
                }
            }

            UnityEngine.Debug.Log("New Tile Layer " + string.Join(", " , temp));

            layer.tiles = temp;

            UnityEngine.Debug.Log("Updated Tile Layer " + layer);

        }

        width = newWidth;
        height = newHeight;
    }

    public void SetTile(int x, int y, int layer, int tileID)
    {
        tileLayers[layer].tiles[x + y * width] = tileID;
    }
}
