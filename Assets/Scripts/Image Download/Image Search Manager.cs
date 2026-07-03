using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Scripts.Image_Download;
using UnityEngine;
using UnityEngine.Networking;

public class ImageSearchManager : MonoBehaviour
{
    public static ImageSearchManager Instance;

    private string query;
    [SerializeField] private GameObject searchPage;
    [SerializeField] private Transform resultsContainer;
    [SerializeField] private ResultButton resultPrefab;
    [SerializeField] private GameObject errorMessage;

    private List<ImageResult> imageResults = new List<ImageResult>();

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
        SearchImages(query);
    }

    public void UpdateQuery(string updatedQuery)
    {
        query = updatedQuery;
    }

    private async void SearchImages(string query)
    {

        //Clear the existing results
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }

        IImageSearchProvider provider =
            new PixabaySearch();

        imageResults = await provider.Search(query);

        //If there are no results, show the error message
        if (imageResults.Count == 0)
        {
            errorMessage.SetActive(true);
            return;
        }

        //If there are results, display them by instantiating the top 50 each result as a button
        errorMessage.SetActive(false);

        int i = 0;
        foreach (ImageResult image in imageResults)
        {
            Sprite sprite = await LoadSpriteFromURL(image.previewURL);
            ResultButton temp = Instantiate<ResultButton>(resultPrefab, resultsContainer);
            temp.Setup(image.title, sprite, image.fullImageURL);
            i++;
            if (i >= 50)
                return;
        }
    }

    public void OpenSearchPage()
    {
        searchPage.SetActive(true);
    }

    public void CloseSearchPage()
    {
        searchPage.SetActive(false);
    }

    public void SelectSearchResult(string name, string downloadURL)
    {
        BackgroundManager.Instance.ImportBackgroundfromURL(name, downloadURL);
        CloseSearchPage();
    }

    public async Task<Sprite> LoadSpriteFromURL(string url)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f);

        return sprite;
    }

}
