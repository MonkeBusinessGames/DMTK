using Assets.Scripts.Campaign;
using Assets.Scripts.Image_Download;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DNDSearchManager : MonoBehaviour
{
    public static DNDSearchManager Instance;

    private string query;
    private Dictionary<string, string> filters;
    [SerializeField] private GameObject searchPage;
    [SerializeField] private GameObject filtersPage;
    [SerializeField] private Transform resultsContainer;
    [SerializeField] private DNDResultButton resultPrefab;
    [SerializeField] private GameObject errorMessage;
    private bool sfx;

    private List<CreatureData> results = new List<CreatureData>();

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects
        Instance = this;
    }

    public void StartSearch()
    {
        SearchDND(query);
    }

    public void UpdateQuery(string updatedQuery)
    {
        query = updatedQuery;
    }
    public void UpdateFilters(string key, string value)
    {
        filters[key] = value;
    }
    public void RemoveFilter(string key)
    {
        if(filters.ContainsKey(key))
            filters.Remove(key);
    }

    private void ClearSearch()
    {

        //Clear the existing results
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private async void SearchDND(string query)
    {
        PanelManager.Instance.Processing(true);
        ClearSearch();

        IDNDSearchProvider provider =
            new Open5ESearch();

        results = await provider.Search(query, filters);

        //If there are no results, show the error message
        if (results.Count == 0)
        {
            errorMessage.SetActive(true);
            PanelManager.Instance.Processing(false);
            return;
        }

        //If there are results, display them by instantiating the top 50 each result as a button
        errorMessage.SetActive(false);

        int i = 0;
        foreach (CreatureData result in results)
        {
            DNDResultButton temp = Instantiate<DNDResultButton>(resultPrefab, resultsContainer);
            temp.Setup(result);
            i++;
            if (i >= 50)
                break;
        }

        PanelManager.Instance.Processing(false);
    }

    public void OpenSearchPage()
    {
        searchPage.SetActive(true);
    }

    public void CloseSearchPage()
    {
        searchPage.SetActive(false);
    }

    public void SelectSearchResult(CreatureData data)
    {
        NPCManager.Instance.NewNPC(data);
        CloseSearchPage();
    }

    public void ToggleFilters()
    {
        filtersPage.SetActive(!filtersPage.activeSelf);
    }
}
