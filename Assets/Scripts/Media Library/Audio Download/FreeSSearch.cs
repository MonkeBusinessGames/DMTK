using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class FreeSSearch : IAudioSearchProvider
{
    private readonly string apiKey = "RbGncTl3dU7cYrI5UAQlD0aJ7fc1aD60tTaVSIxF";

    public FreeSSearch() {}

    public async Task<List<AudioResult>> Search(string query, string filters)
    {
        //Create a request using the URL, API Key and query
        string encodedQuery = UnityWebRequest.EscapeURL(query);

        //Create a request using the URL, API Key and query
        string encodedFilter = UnityWebRequest.EscapeURL(filters);

        string url =
            $"https://freesound.org/apiv2/search/?query={encodedQuery}&token={apiKey}&page_size=50&fields=name,description,username,previews,duration&filter={filters}";

        Debug.Log(url);

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
            return new List<AudioResult>();
        }

        //Else, create a response
        FreeSResponse response =
            JsonConvert.DeserializeObject<FreeSResponse>(request.downloadHandler.text);

        List<AudioResult> results =
            new List<AudioResult>();


        foreach (FreeSResults result in response.results)
        {

            Debug.Log(result);

            results.Add(new AudioResult()
            {
                title = result.name,
                description = result.description,
                creator = result.username,
                downloadURL = result.previews["preview-hq-mp3"].ToString(),
                previewURL = result.previews["preview-lq-mp3"].ToString(),
                duration = result.duration,
            });
        }

        Debug.Log(results);

        return results;
    }
}