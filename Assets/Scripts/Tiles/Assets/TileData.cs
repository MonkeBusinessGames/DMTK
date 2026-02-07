
[System.Serializable]
public class TileData
{
    public string tileName;
    public float tileID;
    public bool isRule;
    public TileRules ruleData;

    public TileData() { }

    public TileData(string name, float id)
    {
        tileName = name;
        tileID = id;
        isRule = false;
        ruleData = null;
    }

    public TileData(string name, float id, TileRules rules)
    {
        tileName = name;
        tileID = id;
        isRule = true;
        ruleData = rules;
    }

    public override string ToString()
    {
        return tileName + " | " + tileID + " | " + isRule + " | " + ruleData;
    }
}
