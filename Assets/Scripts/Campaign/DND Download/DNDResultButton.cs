using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class DNDResultButton: MonoBehaviour
{
    [SerializeField] private TMP_Text buttonName;
    [SerializeField] private TMP_Text buttonDescription;
    private CreatureData data;

    public void Setup(CreatureData creatureData)
    {
        data = creatureData;
        buttonName.text = data.name;
        buttonDescription.text = data.size.name + " " + data.type.name + "| CR " + data.challenge_rating + Environment.NewLine +"Source: " + data.document.display_name;
    }

    public void Info()
    {
    }

    public void Download()
    {
        DNDSearchManager.Instance.SelectSearchResult(data);
    }
}
