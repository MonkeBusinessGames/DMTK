using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Campaign
{
    public class NPCManager : MonoBehaviour
    {
        public static NPCManager Instance;
        public CampaignManager campaign;
        [SerializeField] private GameObject selector;
        public RectTransform content;
        public NPCButton buttonPrefab;

        private void Awake()
        {
            //Prevent duplicates of this object from existing
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            //Make this object accessible to other objects.
            Instance = this;
        }


        public void Delete(string fileName)
        {
            campaign.loadedCampaign.npcs.Remove(fileName);;
            RefreshSelector();
        }

        public void RefreshSelector()
        {

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (var npc in campaign.loadedCampaign.npcs.Values)
            {
                var btn = Instantiate(buttonPrefab, content);
                btn.Setup(npc.name, npc.sprite);
                i++;
                //Debug.Log("new list item " + bg);   
            }

            //Resize scroll content transform
            content.sizeDelta = new Vector2(0, 20 + (180 * i));
        }
        public bool UpdateNPCName(string oldName, string newName)
        {
            if (oldName == newName)
                return true;
            if (campaign.loadedCampaign.npcs.ContainsKey(newName))
                return false;

            NPCData temp = campaign.loadedCampaign.npcs[oldName];
            temp.name = newName;

            campaign.loadedCampaign.npcs.Add(newName, temp);
            campaign.loadedCampaign.npcs.Remove(oldName);

            return true;
        }

        public void UpdateNPCSprite(string npcName, Sprite npcSprite)
        {
            campaign.loadedCampaign.npcs[npcName].sprite = npcSprite;

            //If Initiative is loaded, update sprite
        }

        public void NewNPC()
        {
            campaign.loadedCampaign.npcs.Add("New NPC", new NPCData("New NPC"));
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup("New NPC", null);
        }

        public void NewNPC(CreatureData data)
        {
            campaign.loadedCampaign.npcs.Add(data.name, new NPCData(data));
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(data.name, null);
        }
    }
}