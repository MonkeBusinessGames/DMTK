using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CampaignData
{
    public string campaignName;
    [SerializeField] public Dictionary<string, PlayerData> players = new Dictionary<string, PlayerData>();
    [SerializeField] public Dictionary<string, NPCData> npcs = new Dictionary<string, NPCData>();

    public CampaignData(string newName)
    {
        campaignName = newName;
        players = new Dictionary<string, PlayerData>();
        npcs = new Dictionary<string, NPCData>();
    }

}
