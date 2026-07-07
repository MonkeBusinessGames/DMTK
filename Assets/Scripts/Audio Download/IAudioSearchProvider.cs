using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAudioSearchProvider
{
    Task<List<AudioResult>>
        Search(string query, string filters);
}