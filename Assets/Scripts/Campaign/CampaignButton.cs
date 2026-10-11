using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Campaign
{
    public class CampaignButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonName;
        private string fileName;
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private DataNamer namer;
   
        public void Setup(string file)
        {
            fileName = file;
            buttonName.text = fileName;
        }

        public void UpdateName(string newName)
        {
            if (!namer.RequiredCheck())
                return;
            if (!CampaignManager.Instance.UpdateCampaignName(fileName, newName))
            {
                namer.DuplicateError(fileName);
                return;
            }
            fileName = newName;
            buttonName.text = fileName;
            buttonObject.SetActive(true);
            namer.gameObject.SetActive(false);
        }

        public void EditName()
        {
            buttonObject.SetActive(false);
            namer.gameObject.SetActive(true);
        }

        public void OnClick()
        {
            CampaignManager.Instance.SelectCampaign(fileName);
        }

        public void Delete()
        {
            CampaignManager.Instance.ConfirmDelete(buttonName.text);
        }
    }
}