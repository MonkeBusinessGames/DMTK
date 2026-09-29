using Assets.Scripts.Combat;
using SFB;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InitiativeTracker : MonoBehaviour
{
    public static InitiativeTracker Instance;
    public List<TrackerData> initiativeOrder = new();
    public RectTransform content;
    [SerializeField] private InitiativeButton buttonPrefab;
    [SerializeField] private InitiativeDisplay displayPrefab;

    [SerializeField] private GameObject initiativeDisplay;
    [SerializeField] private TurnTimer turnTimer;
    private int initiativeIndex = 0;

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects.
        Instance = this;
    }


    /// <summary>
    /// Allow users to add a new tracker.
    /// </summary>
    public void AddTracker()
    {
        TrackerData temp = new TrackerData("Tracker " + initiativeOrder.Count);
        initiativeOrder.Add(temp);
        Debug.Log(temp);
        RefreshSelector();
    }

    public void Delete(TrackerData tracker)
    {
        initiativeOrder.Remove(tracker);

        RefreshSelector();
    }

    public void RefreshSelector()
    {

        RefreshDisplay();
     
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach (var tracker in initiativeOrder)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(tracker);
            i++;
            Debug.Log("new list item " + tracker);
        }

        if(initiativeOrder.Count != 0)
            initiativeOrder[initiativeIndex].StartTurn();
    }

    public void MoveTrackerUp(TrackerData tracker)
    {
        int i = initiativeOrder.IndexOf(tracker);
        if (i == 0)
            return;

        initiativeOrder.TrySwap(i,i - 1, out System.Exception error);


        RefreshSelector();
    }

    public void MoveTrackerDown(TrackerData tracker)
    {
        int i = initiativeOrder.IndexOf(tracker);
        if (i == initiativeOrder.Count - 1)
            return;

        initiativeOrder.TrySwap(i, i + 1, out System.Exception error);

        RefreshSelector();
    }

    public void ToggleDisplay(bool toggle)
    {
        initiativeDisplay.SetActive(toggle);
    }

    public void RefreshDisplay()
    {
        foreach (Transform child in initiativeDisplay.transform)
        {
            if (child.name != "Timer")
                Destroy(child.gameObject);
        }
        int i = 0;
        foreach (var tracker in initiativeOrder)
        {
            var btn = Instantiate(displayPrefab, initiativeDisplay.transform);
            btn.Setup(tracker);
            i++;
            Debug.Log("new list item " + tracker);
        }
    }

    public void NextInitiative()
    {
        if (initiativeOrder.Count <= 1)
            return;
        initiativeOrder[initiativeIndex].EndTurn();
        initiativeIndex++;
        if (initiativeIndex >= initiativeOrder.Count)
            initiativeIndex = 0;
        initiativeOrder[initiativeIndex].StartTurn();

    }
    public void PreviousInitiative()
    {
        if (initiativeOrder.Count <= 1)
            return;
        initiativeOrder[initiativeIndex].EndTurn();
        initiativeIndex--;
        if (initiativeIndex < 0)
            initiativeIndex = initiativeOrder.Count - 1;
        initiativeOrder[initiativeIndex].StartTurn();
        turnTimer.ResetTimer();
    }

    public bool CheckDisplay()
    {
        float width = initiativeDisplay.GetComponent<RectTransform>().rect.width;
        float threshold = initiativeOrder.Count * 200;
        return (width > threshold);
        turnTimer.ResetTimer();

    }
}
