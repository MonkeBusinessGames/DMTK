using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackerData
{
    public string trackerName;
    public Sprite trackerSprite;
    public static List<string> trackerNames = new List<string>();

    public InitiativeButton button;
    public InitiativeDisplay display;
    public List<ConditionData> conditions = new List<ConditionData>();

    public TrackerData(string newName)
    {
        trackerName = newName;
        trackerNames.Add(trackerName);
    }

    public bool RenameTracker(string newName)
    {
        if (trackerNames.Contains(newName))
            return false;
        trackerNames.Remove(newName);
        trackerName = newName;
        trackerNames.Add(trackerName);
        display.UpdateName(trackerName);
        return true;
    }

    public void StartTurn()
    {
        display.image.color = button.image.color = Color.grey;
    }
    public void EndTurn()
    {
        display.image.color = button.image.color = Color.white;
    }

    public void UpdateSprite(Sprite sprite)
    {
        trackerSprite = sprite;
        display.icon.sprite = sprite;
        Debug.Log("Updated Sprites for " + display.gameObject.name + " to " + sprite);
    }

    public void AddCondition(ConditionData condition)
    {
        conditions.Add(condition);
        display.RefreshConditions();
    }

    public void RemoveCondition(ConditionData condition)
    {
        conditions.Remove(condition);
        display.RefreshConditions();
    }

    public string ToString()
    {
        return trackerName + " | Sprite: " + trackerSprite.name + " | Button: " + button.gameObject.name + " | Display: " + display.gameObject.name;
    }
}
