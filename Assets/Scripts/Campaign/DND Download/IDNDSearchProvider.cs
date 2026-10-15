using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IDNDSearchProvider
{
    Task<List<CreatureData>>
        Search(string query, Dictionary<string, string> filterList);
}