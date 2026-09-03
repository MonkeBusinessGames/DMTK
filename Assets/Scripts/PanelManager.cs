using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public ManagerData data;

    private string dataPath;

    [SerializeField] private Transform hiddenPanelsParent;
    [SerializeField] private Image leftPanelParent;
    [SerializeField] private Image rightPanelParent;
    [SerializeField] private Dictionary<PanelType, DockPanel> panels;

    private List<DividerController> leftDividers = new List<DividerController>();
    private List<DividerController> rightDividers = new List<DividerController>();
    [SerializeField] private DividerController dividerPrefab;

    public Image widthDivider;

    private float baseHeight = 1080;
    private float baseWidth = 1920;
    private bool isHoveringDivider;
    private bool isDragging;
    private Vector2 localPosition;
    private InputAction leftClick;
    private Vector2 mousePosition;
    private float dragSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects and don't destory it.
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Define the panel path
        dataPath = Path.Combine(Application.persistentDataPath, "Panels");

        //If the folder for storing musics doesn't exist, create it.
        if (!File.Exists(dataPath))
        {
            Debug.Log("New Panel Data");
            data = new ManagerData();
            data.Initialize();
            SavePanelData();
        }
        else
        {
            Debug.Log("Existing Panel Data");
            data = JsonConvert.DeserializeObject<ManagerData>(File.ReadAllText(dataPath));
        }

        InitiatePanels();

        //Define leftclick
        leftClick = InputSystem.actions.FindAction("LeftClick");
    }

    private void Update()
    {

        //Click and drag the camera
        if (!isDragging)
        {
            //Check Mouse is in the image
            RectTransform imageRect = widthDivider.rectTransform;
            bool nowHovering;
            //Get the local position and whether we are hovering over the scene image
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, Mouse.current.position.ReadValue(), null, out localPosition))
            {
                nowHovering = false;
            }

            nowHovering = imageRect.rect.Contains(localPosition);

            //If the mouse is not over the rect, don't bother continuing the rest of the updates
            if (!nowHovering)
            {
                if(isHoveringDivider)
                    CursorController.Instance.RevertCursor();
                isHoveringDivider = nowHovering;
                return;
            }

            if(!isHoveringDivider)
                CursorController.Instance.SetCursor(ToolState.HDrag);

            isHoveringDivider = nowHovering;

            //If a click is detecting, start dragging
            if (leftClick.IsPressed())
            {
                isDragging = true;
                mousePosition = Camera.main.ScreenToViewportPoint(Mouse.current.position.value);
            }
        }
        else
        {
            //If a click is still detected, drag by the mouse movement delta times a dampener value
            if (leftClick.IsPressed())
            {
                //Vector2 newPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                //Debug.Log(Camera.main.pixelWidth);
                //data.UpdateWidth((mousePosition.x - newPosition.x) * dragSpeed);
                //Debug.Log((mousePosition.x - newPosition.x) * dragSpeed);
                ////Set Panel widths
                //leftPanelParent.preferredWidth = data.leftWidth * baseWidth;
                //rightPanelParent.preferredWidth = data.rightWidth * baseWidth;

                //mousePosition = newPosition;

                mousePosition = Camera.main.ScreenToViewportPoint(Mouse.current.position.value);
                //Debug.Log(mousePosition);
                if(mousePosition.x > .1 && mousePosition.x < .9)
                {
                    widthDivider.rectTransform.anchorMin = new Vector2(mousePosition.x, 0);
                    widthDivider.rectTransform.anchorMax = new Vector2(mousePosition.x, 1);
                    leftPanelParent.rectTransform.anchorMax = new Vector2(mousePosition.x, 1);
                    rightPanelParent.rectTransform.anchorMin = new Vector2(mousePosition.x, 0);

                    data.horizontalRatio = mousePosition.x;
                }
                    
            }
            //If a click is not detected, stop dragging
            else
            {
                isDragging = false;
            }
        }
    }

    private void SavePanelData()
    {
        File.WriteAllText(dataPath, JsonConvert.SerializeObject(data));
    }


    private void InitiatePanels()
    {
        //Set Panel widths
        if(data.leftPanels.Count == 0)
        {
            widthDivider.gameObject.SetActive(false);
            leftPanelParent.rectTransform.anchorMax = new Vector2(0, 0);
            rightPanelParent.rectTransform.anchorMin = new Vector2(0, 0);
            Debug.Log("L: " + data.leftPanels.Count + " | R: " + data.rightPanels.Count + " | Ratio: N/A");

        }
        else if(data.rightPanels.Count == 0)
        {
            widthDivider.gameObject.SetActive(false);
            leftPanelParent.rectTransform.anchorMax = new Vector2(1, 1);
            rightPanelParent.rectTransform.anchorMin = new Vector2(1, 1);
            Debug.Log("L: " + data.leftPanels.Count + " | R: " + data.rightPanels.Count + " | Ratio: N/A");

        }
        else
        {
            widthDivider.gameObject.SetActive(true);
            widthDivider.rectTransform.anchorMin = new Vector2(data.horizontalRatio, 0);
            widthDivider.rectTransform.anchorMax = new Vector2(data.horizontalRatio, 1);
            leftPanelParent.rectTransform.anchorMax = new Vector2(data.horizontalRatio, 1);
            rightPanelParent.rectTransform.anchorMin = new Vector2(data.horizontalRatio, 0);
            Debug.Log("L: " + data.leftPanels.Count + " | R: " + data.rightPanels.Count + " | Ratio: " + data.horizontalRatio);

        }

        //Start the counter to calculate anchors
        float anchorCounter = 1;

        //Set column counts
        DockPanel.leftCount = data.leftPanels.Count;
        DockPanel.rightCount = data.rightPanels.Count;

        //Clear Left Panels
        foreach (Transform child in leftPanelParent.transform)
        {
            if(child.TryGetComponent<DockPanel>(out DockPanel test))
                child.transform.SetParent(hiddenPanelsParent.transform, false);
            else
                Destroy(child.gameObject);
        }

        //Initialize Left Panels, Dividers and Set Heights
        for (int i = 0; i < data.leftPanels.Count; i++)
        {
            panels[data.leftPanels[i]].transform.SetParent(leftPanelParent.transform, false);
            panels[data.leftPanels[i]].ResetRect();

            if (i > 0)
            {
                anchorCounter -= data.leftHeights[i - 1];
                var divider = Instantiate(dividerPrefab, leftPanelParent.transform);
                divider.Initialize(panels[data.leftPanels[i - 1]], panels[data.leftPanels[i]], anchorCounter);
                leftDividers.Add(divider);
            }
            
            //Initialize the panel on the left
            panels[data.leftPanels[i]].Initialize(true, i, anchorCounter, anchorCounter - data.leftHeights[i]);           
        }

        //Reset the anchor counter
        anchorCounter = 1;

        //Clear Right Panels
        foreach (Transform child in rightPanelParent.transform)
        {
            if (child.TryGetComponent<DockPanel>(out DockPanel test))
                child.transform.SetParent(hiddenPanelsParent.transform, false);
            else
            {
                Destroy(child.gameObject);
            }
        }

        //Initialize Right Panels, Dividers and Set Heights
        for (int i = 0; i < data.rightPanels.Count; i++)
        {
            panels[data.rightPanels[i]].transform.SetParent(rightPanelParent.transform, false);
            panels[data.rightPanels[i]].ResetRect();
            Debug.Log("New Panel added to right " + data.rightPanels[i]);

            if (i > 0)
            {
                anchorCounter -= data.rightHeights[i - 1];
                var divider = Instantiate(dividerPrefab, rightPanelParent.transform);
                divider.Initialize(panels[data.rightPanels[i - 1]], panels[data.rightPanels[i]], anchorCounter);
                rightDividers.Add(divider); 
            }

            //Initialize the panel on the right
            panels[data.rightPanels[i]].Initialize(false, i, anchorCounter, anchorCounter - data.rightHeights[i]);
        }
    }

    public void AddPanel(PanelType type)
    {
        //Add the new panel to the column with less panels
        data.AddPanel(type, data.rightPanels.Count > data.leftPanels.Count);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

    public void AddPanel(int type)
    {
        if (data.leftPanels.Contains((PanelType)type) || data.rightPanels.Contains((PanelType)type))
            return;

        //Add the new panel to the column with less panels
        data.AddPanel((PanelType) type, data.rightPanels.Count > data.leftPanels.Count);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

    public void RemovePanel(PanelType type, bool left)
    {
        //Remove the panel from the corresponding column
        data.RemovePanel(type, left);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

    public void MoveUp(PanelType type, bool left)
    {
        //Update the panel from the corresponding column
        data.MovePanelUp(type, left);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

    public void MoveDown(PanelType type, bool left)
    {
        //Update the panel from the corresponding column
        data.MovePanelDown(type, left);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

    public void MoveSideways(PanelType type, bool left)
    {
        //Remove the panel from the corresponding column
        data.RemovePanel(type, left);

        //Add the panel to the other column
        data.AddPanel(type, !left);

        //Reset the panels in the workspace
        InitiatePanels();

        SavePanelData();
    }

}
