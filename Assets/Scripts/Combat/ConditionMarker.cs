using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConditionMarker : MonoBehaviour
{
    [SerializeField] private Image conditionIcon;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text conditionText;
    private TrackerData tracker;
    public ConditionData data;

    public void Setup(TrackerData newTracker, ConditionData conditionData)
    {
        tracker = newTracker;
        data = conditionData;
        conditionText.text = data.text;
        conditionIcon.sprite = data.icon;
        background.color = data.color;
    }
    public void Setup(TrackerData newTracker, ConditionData conditionData, bool showText)
    {
        tracker = newTracker;
        data = conditionData;
        conditionText.text = data.text;
        conditionIcon.sprite = data.icon;
        background.color = data.color;
        conditionText.gameObject.SetActive(showText);
    }

    public void RemoveCondition()
    {
        tracker.RemoveCondition(data);
        Destroy(this.gameObject);
    }
}
