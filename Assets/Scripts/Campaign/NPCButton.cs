using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.U2D.Physics.PhysicsLayers;

namespace Assets.Scripts.Campaign
{
    public class NPCButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonName;
        private string npcName;
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private DataNamer namer;

        [SerializeField] private CustomDropdown iconDropdown;
        public Image image;

        public void Setup(string file, Sprite npcSprite)
        {
            npcName = file;
            buttonName.text = npcName;
            namer.SetName(npcName);

            if (npcSprite == null)
                UpdateSprite(iconDropdown.defaultOption);
            else
                iconDropdown.previewImage.sprite = npcSprite;
        }

        public void UpdateName(string newName)
        {
            if (!namer.RequiredCheck())
                return;
            if (!NPCManager.Instance.UpdateNPCName(npcName, newName))
            {
                namer.DuplicateError(npcName);
                return;
            }
            npcName = newName;
            buttonName.text = npcName;
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
            InitiativeTracker.Instance.AddTracker(npcName, iconDropdown.previewImage.sprite);
        }

        public void UpdateSprite(CustomDropdownOption option)
        {
            NPCManager.Instance.UpdateNPCSprite(npcName, option.icon);
            iconDropdown.previewImage.sprite = option.icon;
        }

        public void Delete()
        {
            NPCManager.Instance.Delete(npcName);
        }
    }
}