using UnityEngine;

[System.Serializable]
public class TileData
{
    public string tileName;
    public bool isRule;
    public TileRules ruleData;

    public TileData(string name)
    {
        tileName = name;
        isRule = false;
        ruleData = null;
    }

    public TileData(string name, TileRules rules)
    {
        tileName = name;
        isRule = true;
        ruleData = rules;
    }

    public override string ToString()
    {
        return tileName + " | " + isRule + " | " + ruleData;
    }
}
