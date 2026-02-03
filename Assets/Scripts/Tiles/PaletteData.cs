using UnityEngine;
using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine.UIElements;


[System.Serializable]
public class PaletteData : MonoBehaviour
{
    public string paletteName;
    public string palettePath;
    public Dictionary<string, TileData> tList;
    public string mainSprite;

    public PaletteData(string name, Dictionary<string, TileData> tiles, string sprite)
    {
        paletteName = name;
        palettePath = Path.Combine(Application.persistentDataPath, "Palettes", paletteName);
        tList = tiles;
        mainSprite = sprite;
    }
}
