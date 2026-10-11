using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{

    public string playerName;
    public bool customIcon;

    public Sprite sprite;
    public PlayerData(string newName)
    {
        playerName = newName;
        customIcon = false;
        sprite = null;
    }
}
