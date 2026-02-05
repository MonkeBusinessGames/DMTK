using UnityEngine;
using System.IO;
using System.Collections.Generic;


[System.Serializable]
public class PaletteData
{
    public string paletteName;
    public string palettePath;
    public Dictionary<string, TileData> tList;
    public string mainSprite;

    public PaletteData()
    {
        paletteName = "None";
        palettePath = "None";
        tList = new Dictionary<string, TileData>();
        mainSprite = null;
    }

    public PaletteData(string name, Dictionary<string, TileData> tiles, string sprite)
    {
        paletteName = name;
        palettePath = Path.Combine(Application.persistentDataPath, "Palettes", paletteName);
        tList = tiles;
        mainSprite = sprite;
    }

    public override string ToString()
    {
        string t = "";
        foreach(var key in tList.Keys)
        {
            t += ", " + key;
        }

        return paletteName + " | " + palettePath + " | " + t + " | " + mainSprite;
    }

}
