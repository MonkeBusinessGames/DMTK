using Assets.Scripts.Campaign;
using Assets.Scripts.Player;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;
    [SerializeField] private TMP_Dropdown settingsDropdown;
    private int settingsIndex;
    [SerializeField] private GameObject campaignSelector;
    [SerializeField] private GameObject playerSelector;
    [SerializeField] private GameObject npcSelector;
    [SerializeField] private GameObject deleteConfirmation;
    private string deletableCampaign;
    private string campaignPath;
    public List<string> campaignList = new();
    public RectTransform content;
    public CampaignButton buttonPrefab;
    public CampaignData loadedCampaign;
    public TMP_Text currentCampaignText;
    public Button saveCampaign;


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

        //Define the campaign path
        campaignPath = Path.Combine(Application.persistentDataPath, "Campaigns");

        //If the folder for storing campaigns doesn't exist, create it.
        if (!Directory.Exists(campaignPath))
        {
            Directory.CreateDirectory(campaignPath);
        }

        foreach (var campaign in Directory.GetFiles(campaignPath))
        {
            campaignList.Add(Path.GetFileNameWithoutExtension(campaign));
        }
        //Refresh the campaigns list
        RefreshSelector();

    }
    private void Start()
    {
        settingsDropdown.gameObject.SetActive(false);
        SelectMedia(0);
        settingsDropdown.SetValueWithoutNotify(0);
    }

    public void SelectMedia(int media)
    {
        settingsIndex = media;
        switch (settingsIndex)
        {
            case 0: // campaigns
                campaignSelector.SetActive(true);
                playerSelector.SetActive(false); 
                npcSelector.SetActive(false);

                break;
            case 1: // players
                campaignSelector.SetActive(false);
                playerSelector.SetActive(true);
                npcSelector.SetActive(false);
                break;
            case 2: // npcs
                campaignSelector.SetActive(false);
                playerSelector.SetActive(false);
                npcSelector.SetActive(true);
                break;
        }
    }

    public void AddNew()
    {
        switch (settingsIndex)
        {
            case 0: // campaign
                NewCampaign();
                break;
            case 1: // player
                PlayerManager.Instance.NewPlayer();
                break;
            case 2: // npc
                NPCManager.Instance.NewNPC();
                break;
        }
    }

    public void ConfirmDelete(string fileName)
    {
        deletableCampaign = fileName;
        deleteConfirmation.SetActive(true);
    }

    public void CancelDelete()
    {
        deleteConfirmation.SetActive(false);
    }

    public void Delete()
    {
        if (deletableCampaign == loadedCampaign.campaignName)
        {
            loadedCampaign = null;
            currentCampaignText.text = "";
            saveCampaign.interactable = false;
            settingsDropdown.gameObject.SetActive(false);
        }

        File.Delete(Path.Combine(campaignPath, deletableCampaign));
        campaignList.Remove(deletableCampaign);
        RefreshSelector();
        deleteConfirmation.SetActive(false);
    }

    public void RefreshSelector()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var bg in campaignList)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(bg);
            i++;
            //Debug.Log("new list item " + bg);   
        }

        //Resize scroll content transform
        content.sizeDelta = new Vector2(0, 20 + (180 * i));
    }

    public void SaveCampaign()
    {
        File.WriteAllText(Path.Combine(campaignPath, loadedCampaign.campaignName), JsonConvert.SerializeObject(loadedCampaign));
    }

    public bool UpdateCampaignName(string oldName, string newName)
    {
        if (oldName == newName)
            return true;
        if (campaignList.Contains(newName))
            return false;

        campaignList[campaignList.IndexOf(oldName)] = newName;

        if(loadedCampaign.campaignName == oldName)
        {
            File.Delete(Path.Combine(campaignPath, oldName));
            loadedCampaign.campaignName = newName;
            currentCampaignText.text = newName;
            File.WriteAllText(Path.Combine(campaignPath, newName), JsonConvert.SerializeObject(loadedCampaign));
            return true;
        }

        CampaignData temp = JsonConvert.DeserializeObject<CampaignData>(File.ReadAllText(Path.Combine(campaignPath, oldName)));

        File.Delete(Path.Combine(campaignPath, oldName));
        temp.campaignName = newName;
        File.WriteAllText(Path.Combine(campaignPath, newName), JsonConvert.SerializeObject(temp));

        return true;
    }

    public void NewCampaign()
    {
        CampaignData temp = new CampaignData("New Campaign");
        var btn = Instantiate(buttonPrefab, content);
        btn.Setup("New Campaign");
        campaignList.Add("New Campaign"); 
        File.WriteAllText(Path.Combine(campaignPath, temp.campaignName), JsonConvert.SerializeObject(temp));

    }

    public void SelectCampaign(string campaignName)
    {
        if(loadedCampaign.campaignName == campaignName)
            return;
        settingsDropdown.gameObject.SetActive(true);
        loadedCampaign = JsonConvert.DeserializeObject<CampaignData>(File.ReadAllText(Path.Combine(campaignPath, campaignName)));
        currentCampaignText.text = campaignName;
        saveCampaign.interactable = true;
        PlayerManager.Instance.RefreshSelector();
        NPCManager.Instance.RefreshSelector();
    }
}
