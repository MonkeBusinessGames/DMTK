using UnityEngine;

[System.Serializable]
public class TileData
{
    public string tileName;
    public string palette;
    public bool isRule;
    public TileRules ruleData;

    public TileData(string name, string tilepalette)
    {
        tileName = name;
        palette = tilepalette;
        isRule = false;
        ruleData = null;
    }

    public TileData(string name, string tilepalette, TileRules rules)
    {
        tileName = name;
        palette = tilepalette;
        isRule = true;
        ruleData = rules;
    }
}
