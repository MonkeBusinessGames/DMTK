using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData : CreatureData
{
    public bool customIcon;

    public Sprite sprite;
    public PlayerData(string newName)
    {
        name = newName;
        customIcon = false;
        sprite = null;
    }
}
