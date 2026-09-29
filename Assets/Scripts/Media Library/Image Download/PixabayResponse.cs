using System;
using System.Collections.Generic;

[Serializable]
public class PixabayResponse
{
    public int total;
    public int totalHits;
    public List<PixabayHit> hits;
}

[Serializable]
public class PixabayHit
{
    public string previewURL;

    public string largeImageURL;

    public string user;
}