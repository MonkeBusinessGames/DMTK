using UnityEngine;
using System.IO;
using System.Collections.Generic;


[System.Serializable]
public class PaletteData
{
    public string paletteName;
    public string palettePath;
    public Dictionary<int, string> tList;
    public string mainSprite;

    public PaletteData()
    {
        paletteName = "";
        palettePath = "";
        tList = new Dictionary<int, string>();
        mainSprite = null;
    }

    public PaletteData(string path)
    {
        paletteName = "";
        palettePath = Path.Combine(path, "temp");
        tList = new Dictionary<int, string>();
        mainSprite = null;
    }

    public PaletteData(string name, Dictionary<int, string> tiles, string sprite)
    {
        paletteName = name;
        palettePath = Path.Combine(Application.persistentDataPath, "Palettes", paletteName);
        tList = tiles;
        mainSprite = sprite;
    }

    public override string ToString()
    {
        string t = "";
        foreach(var key in tList.Values)
        {
            t += ", " + key;
        }

        return paletteName + " | " + palettePath + " | " + t + " | " + mainSprite;
    }
}
