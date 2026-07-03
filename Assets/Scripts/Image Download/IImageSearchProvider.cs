using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IImageSearchProvider
{
    Task<List<ImageResult>>
        Search(string query);
}