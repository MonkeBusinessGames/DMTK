using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Campaign
{
    public class CampaignButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonName;
        private string campaignName;
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private DataNamer namer;
   
        public void Setup(string file)
        {
            campaignName = file;
            buttonName.text = campaignName;
            namer.SetName(campaignName);
        }

        public void UpdateName(string newName)
        {
            if (!namer.RequiredCheck())
                return;
            if (!CampaignManager.Instance.UpdateCampaignName(campaignName, newName))
            {
                namer.DuplicateError(campaignName);
                return;
            }
            campaignName = newName;
            buttonName.text = campaignName;
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
            CampaignManager.Instance.SelectCampaign(campaignName);
        }

        public void Delete()
        {
            CampaignManager.Instance.ConfirmDelete(buttonName.text);
        }
    }
}