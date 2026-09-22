using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class ConditionData
{
    public Sprite icon;
    public string text;
    public Color color;

    public ConditionData()
    {
        icon = null;
        text = "";
        color = Color.white;
    }

    public ConditionData(Sprite conditionSprite, string conditionText, Color conditionColor)
    {
        icon = conditionSprite;
        text = conditionText;
        color = conditionColor;
    }

    public string ToString()
    {
        return icon.ToString() + "|" + text + "|" + color.ToHexString();
    }
}