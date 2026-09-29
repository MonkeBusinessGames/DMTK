using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Image_Download;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioSearchManager : MonoBehaviour
{
    public static AudioSearchManager Instance;

    private string query;
    private string filter;
    [SerializeField] private GameObject searchPage;
    [SerializeField] private TMP_Dropdown categories;
    private string[] categoryList = { "Music", "\"Instrument%20samples\"", "Speech", "\"Sound%20effects\"", "Soundscapes" };
    [SerializeField] private Transform resultsContainer;
    [SerializeField] private AudioResultButton resultPrefab;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private AudioSource previewSource;
    Dictionary<string, AudioClip> previewCache = new();
    private bool sfx;

    private List<AudioResult> results = new List<AudioResult>();

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects and don't destory it.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartSearch()
    {
        SearchAudio(query);
    }

    public void UpdateQuery(string updatedQuery)
    {
        query = updatedQuery;
    }
    public void UpdateCategories()
    {

        filter = "category:(";
        bool firstCat = true;
        for (int i = 0; i < 5; i++)
        {
            bool selected = (categories.value & (1 << i)) != 0;

            if (selected)
            {
                if (firstCat)
                {
                    filter += categoryList[i];
                    firstCat = false;
                }
                else
                    filter += "%20OR%20" + categoryList[i];
            }
        }

        filter += ")";

        Debug.Log(filter);
     }

    private void ClearSearch()
    {

        //Clear the existing results
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }

        //Clear preview Cache
        foreach (AudioClip clip in previewCache.Values)
        {
            Destroy(clip);
        }
        previewCache.Clear();
    }

    private async void SearchAudio(string query)
    {

        ClearSearch();

        IAudioSearchProvider provider =
            new FreeSSearch();

        results = await provider.Search(query, filter);

        //If there are no results, show the error message
        if (results.Count == 0)
        {
            errorMessage.SetActive(true);
            return;
        }

        //If there are results, display them by instantiating the top 50 each result as a button
        errorMessage.SetActive(false);

        int i = 0;
        foreach (AudioResult audio in results)
        {
            AudioResultButton temp = Instantiate<AudioResultButton>(resultPrefab, resultsContainer);
            temp.Setup(audio.title, audio.description, audio.downloadURL, audio.previewURL, audio.duration);
            i++;    
            if (i >= 50)
                return;
        }
    }

    public void OpenSearchPage(bool forSFX)
    {
        if(sfx != forSFX)
        {
            ClearSearch();
            sfx = forSFX;         
        }

        if (sfx)
            categories.value = 10;
        else
            categories.value = 17;

        searchPage.SetActive(true);
    }

    public void CloseSearchPage()
    {
        searchPage.SetActive(false);
    }

    public void SelectSearchResult(string name, string downloadURL)
    {
        if (sfx)
        {
            SFXManager.Instance.ImportSFXfromURL(name, downloadURL);
        }
        else
        {
            MusicManager.Instance.ImportMusicfromURL(name, downloadURL);
        }
        CloseSearchPage();
    }

    public async void PlayPreview(string preview, CancellationToken token)
    {
        AudioClip clip = await LoadPreview(preview, token);
        if (clip == null)
            return;

        PlayClip(clip, token);

    }

    public async Task<AudioClip> LoadPreview(string url, CancellationToken token)
    {try
        {

            Debug.Log(AudioResultButton.isPlaying + " started loading");

            if (previewCache.TryGetValue(url, out var cached))
                return cached;

            using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            
            var op = req.SendWebRequest();
            while (!op.isDone)
            {
                token.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
                return null;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
            previewCache[url] = clip;

            Debug.Log(AudioResultButton.isPlaying + " finished loading");

            return clip;
        }
        catch (OperationCanceledException)
        {
            Debug.Log(AudioResultButton.isPlaying + " was cancelled while loading");
            previewSource.Stop();
            AudioResultButton.isPlaying.Cancel();
            return null;
        }
    }

    public async Task PlayClip(AudioClip clip, CancellationToken token)
    {
        AudioResultButton.isPlaying.loading.SetTrigger("Loaded");
        previewSource.PlayOneShot(clip);

        Debug.Log(AudioResultButton.isPlaying + " started playing");

        try
        {
            while (previewSource.isPlaying)
            {
                token.ThrowIfCancellationRequested();
                await Task.Yield();
            }
            Debug.Log(AudioResultButton.isPlaying + " finished playing");
            AudioResultButton.isPlaying.End();
        }


        catch (OperationCanceledException)
        {
            Debug.Log(AudioResultButton.isPlaying + " was cancelled while playing");
            previewSource.Stop();
            AudioResultButton.isPlaying.Cancel();
            return;
        }

    }

}
