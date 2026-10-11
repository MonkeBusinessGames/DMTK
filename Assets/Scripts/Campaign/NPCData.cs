using UnityEngine;


[System.Serializable]
public class NPCData
{
    public string npcName;
    public bool customIcon;
    public Sprite sprite;
    public NPCData(string newName)
    {
        npcName = newName;
        customIcon = false;
        sprite = null;
    }
}
