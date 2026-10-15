using Assets.Scripts.Campaign;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public CampaignManager campaign;
        [SerializeField] private GameObject selector;
        public RectTransform content;
        public PlayerButton buttonPrefab;

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
            campaign.loadedCampaign.players.Remove(fileName);
            RefreshSelector();
        }

        public void OpenSelector()
        {
            selector.SetActive(true);
            DMManager.onGrid = false;
        }
        public void CloseSelector()
        {
            selector.SetActive(false);
            DMManager.onGrid = true;
        }

        public void RefreshSelector()
        {

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            int i = 0;

            foreach (var player in campaign.loadedCampaign.players.Values)
            {
                var btn = Instantiate(buttonPrefab, content);
                btn.Setup(player.name, player.sprite);
                i++;
            }

            //Resize scroll content transform
            content.sizeDelta = new Vector2(0, 20 + (180 * i));
        }

        public bool UpdatePlayerName(string oldName, string newName)
        {
            if (oldName == newName)
                return true;
            if (campaign.loadedCampaign.players.ContainsKey(newName))
                return false;

            PlayerData temp = campaign.loadedCampaign.players[oldName];
            temp.name = newName;

            campaign.loadedCampaign.players.Add(newName, temp);
            campaign.loadedCampaign.players.Remove(oldName);

            return true;
        }

        public void UpdatePlayerSprite(string playerName, Sprite playerSprite)
        {
            campaign.loadedCampaign.players[playerName].sprite = playerSprite;

            //If Initiative is loaded, update sprite
        }

        public void NewPlayer()
        {
            campaign.loadedCampaign.players.Add("New Player", new PlayerData("New Player"));
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup("New Player", null);
        }
    }
}