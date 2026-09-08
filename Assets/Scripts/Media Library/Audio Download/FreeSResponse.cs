using System;
using System.Collections.Generic;

[Serializable]
public class FreeSResponse
{
    public int count;
    public string next;
    public string previous;
    public List<FreeSResults> results;
}

[Serializable]
public class FreeSResults
{
    public string name;

    public string description;

    public string username;

    public Dictionary<string, Uri> previews;

    public float duration;

    public override string ToString()
    {
        return name + "," + description + "," + username + "," + previews["preview-hq-mp3"] + "," + previews["preview-lq-mp3"] + "," + duration;
    }
}