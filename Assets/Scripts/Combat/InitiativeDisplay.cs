using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InitiativeDisplay : MonoBehaviour
{

    [SerializeField] private TMP_Text displayName;
    public Image image;
    public Image icon;
    [SerializeField] private ConditionMarker conditionPrefab;
    [SerializeField] private Transform conditionParent;

    private TrackerData tracker;
    public static bool showText;
    private bool showConditionText;

    public void Setup(TrackerData newTracker)
    {
        tracker = newTracker;
        displayName.text = tracker.trackerName;
        icon.sprite = tracker.trackerSprite;
        tracker.display = this;

        showConditionText = CheckConditionsDisplay();
        displayName.gameObject.SetActive(showText);

        foreach (var condition in tracker.conditions)
        {
            var btn = Instantiate(conditionPrefab, conditionParent);
            btn.Setup(tracker, condition);
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if(CheckConditionsDisplay() != showConditionText)
        {
            showConditionText = !showConditionText;
            RefreshConditions();
        }

        if ((InitiativeTracker.Instance.CheckDisplay() != InitiativeDisplay.showText))
        {
            showText = !showText;
            InitiativeTracker.Instance.RefreshDisplay();
        }
    }
   
    private bool CheckConditionsDisplay()
    {
        float width = GetComponent<RectTransform>().rect.width;
        float threshold = tracker.conditions.Count * 160;
        return (width > threshold);

    }

    public void RefreshConditions()
    {

        foreach (Transform child in conditionParent)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach (var condition in tracker.conditions)
        {
            var btn = Instantiate(conditionPrefab, conditionParent);
            btn.Setup(tracker, condition, showConditionText);
            i++;
            Debug.Log("new list item " + tracker);
        }
    }

    public void UpdateName(string name)
    {
        displayName.text = name;
    }
}
