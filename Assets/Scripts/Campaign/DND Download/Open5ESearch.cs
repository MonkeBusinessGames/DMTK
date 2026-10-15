using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Open5ESearch : IDNDSearchProvider
{
    public Open5ESearch() {}

    public async Task<List<CreatureData>> Search(string query, Dictionary<string, string> filterList)
    {
        //Create a request using the URL, API Key and query
        string encodedQuery = UnityWebRequest.EscapeURL(query);

        string filters = "";

        //foreach(var filter in filterList)
        //{
        //    filters += filter.Key + "=" + filter.Value + "&";
        //}

        string encodedFilters = UnityWebRequest.EscapeURL(filters);


        string url =
            $"https://api.open5e.com/v2/creatures/?{encodedFilters}name__icontains={encodedQuery}";

        Debug.Log("Fetching DND data from " + url);

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
            return new List<CreatureData>();
        }

        //Else, create a response
        Debug.Log(request.downloadHandler.text);
        Open5EResponse response =
            JsonConvert.DeserializeObject<Open5EResponse>(request.downloadHandler.text);

        List<CreatureData> results =
            new List<CreatureData>();

        foreach (Open5EResult result in response.results)
        {
            results.Add(new CreatureData()
            {
                key = result.key,
                name = result.name,
                document = result.document,
                category = result.category,
                subcategory = result.subcategory,
                alignment = result.alignment,
                type = result.type,
                size = result.size,
                challenge_rating = result.challenge_rating,
                proficiency_bonus = result.proficiency_bonus,
                experience_points = result.experience_points,
                armor_class = result.armor_class,
                armor_detail = result.armor_detail,
                hit_points = result.hit_points,
                hit_dice = result.hit_dice,
                initiative_bonus = result.initiative_bonus,
                speed = result.speed_all,
                ability_scores = result.ability_scores,
                senses = new SensesData()
                {
                    normalSightRange = result.normalSightRange,
                    darkvisionRange = result.darkvisionRange,
                    blindsightRange = result.blindsightRange,
                    tremorsenseRange = result.tremorsenseRange,
                    truesightRange = result.truesightRange,
                },
                resistances = result.resistances_and_immunities,
                languages = result.languages,
                traits = result.traits,
                actions = result.actions,
            });

        }

        return results;
    }
}