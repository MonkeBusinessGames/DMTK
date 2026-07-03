using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class PixabaySearch : IImageSearchProvider
{
    private readonly string apiKey = "56540661-eb2ba07370f9db81f37335530";

    public PixabaySearch() {}

    public async Task<List<ImageResult>> Search(string query)
    {
        //Create a request using the URL, API Key and query
        string encodedQuery = UnityWebRequest.EscapeURL(query);

        string url =
            $"https://pixabay.com/api/?key={apiKey}&q={encodedQuery}&image_type=all&safesearch=true&category=backgrounds&orientation=horizontal";

        //Send the web request
        using UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        //Wait for a response from the web request
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        //If the response is a failure, show an empty list
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return new List<ImageResult>();
        }

        //Else, create a response
        PixabayResponse response =
            JsonConvert.DeserializeObject<PixabayResponse>(request.downloadHandler.text);

        List<ImageResult> results =
            new List<ImageResult>();

        foreach (PixabayHit hit in response.hits)
        {
            results.Add(new ImageResult()
            {
                title = hit.previewURL.Split("/")[^1],
                previewURL = hit.previewURL,
                fullImageURL = hit.largeImageURL,
                author = hit.user
            });
        }

        return results;
    }
}