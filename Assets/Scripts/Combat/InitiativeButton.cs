using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class InitiativeButton : MonoBehaviour
{

    [SerializeField] private TMP_Text buttonName;
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private DataNamer namer;
    private TrackerData tracker;
    [SerializeField] private CustomDropdown iconDropdown;
    public Image image;
    [SerializeField] private ConditionMarker conditionPrefab;
    [SerializeField] private Transform conditionParent;

    public void Setup(TrackerData newTracker)
    {
        tracker = newTracker;
        buttonName.text = tracker.trackerName;
        tracker.button = this;
        if (tracker.trackerSprite == null)
            UpdateSprite(iconDropdown.defaultOption);
        else
            iconDropdown.previewImage.sprite = tracker.trackerSprite;

        foreach(var condition in tracker.conditions)
        {
            var btn = Instantiate(conditionPrefab, conditionParent);
            btn.Setup(tracker, condition);
        }
    }

    public void Delete()
    {
        InitiativeTracker.Instance.Delete(tracker);
    }

    public void MoveUp()
    {
        InitiativeTracker.Instance.MoveTrackerUp(tracker);

    }

    public void MoveDown()
    {
        InitiativeTracker.Instance.MoveTrackerDown(tracker);
    }

    public void UpdateName(string newName)
    {
        if (!namer.RequiredCheck())
            return;
        if(!tracker.RenameTracker(newName))
        {
            namer.DuplicateError(tracker.trackerName);
            return;
        }
        buttonName.text =tracker.trackerName;
        buttonObject.SetActive(true);
        namer.gameObject.SetActive(false);
    }

    public void EditName()
    {
        buttonObject.SetActive(false);
        namer.gameObject.SetActive(true);
    }

    public void UpdateSprite(CustomDropdownOption option)
    {
        tracker.UpdateSprite(option.icon);
        iconDropdown.previewImage.sprite = tracker.trackerSprite;
    }

    public void AddConditions(CustomDropdownOption option)
    {
        ConditionData condition = new ConditionData(option.icon, option.label, option.backgroundColor);
        Debug.Log(condition);
        tracker.AddCondition(condition);
        var btn = Instantiate(conditionPrefab, conditionParent);
        btn.Setup(tracker, condition);
    }
}
