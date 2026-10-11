using Assets.Scripts.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Campaign
{
    public class PlayerButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonName;
        private string playerName;
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private DataNamer namer;

        [SerializeField] private CustomDropdown iconDropdown;
        public Image image;

        public void Setup(string file, Sprite playerSprite)
        {
            playerName = file;
            buttonName.text = playerName;

            if (playerSprite == null)
                UpdateSprite(iconDropdown.defaultOption);
            else
                iconDropdown.previewImage.sprite = playerSprite;
        }

        public void UpdateName(string newName)
        {
            if (!namer.RequiredCheck())
                return;
            if (!PlayerManager.Instance.UpdatePlayerName(playerName, newName))
            {
                namer.DuplicateError(playerName);
                return;
            }
            playerName = newName;
            buttonName.text = playerName;
            buttonObject.SetActive(true);
            namer.gameObject.SetActive(false);
        }

        public void EditName()
        {
            buttonObject.SetActive(false);
            namer.gameObject.SetActive(true);
        }

        public void AddToInitiative()
        {
            InitiativeTracker.Instance.AddTracker(playerName, iconDropdown.previewImage.sprite);
        }
        public void UpdateSprite(CustomDropdownOption option)
        {
            PlayerManager.Instance.UpdatePlayerSprite(playerName, option.icon);
            iconDropdown.previewImage.sprite = option.icon;
        }

        public void Delete()
        {
            PlayerManager.Instance.Delete(playerName);
        }
    }
}