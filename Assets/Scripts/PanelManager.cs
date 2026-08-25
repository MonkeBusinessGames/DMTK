using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
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
            data = new ManagerData();
            SavePanelData();
        }
        else
            data = JsonConvert.DeserializeObject<ManagerData>(File.ReadAllText(dataPath));

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
                Debug.Log(mousePosition);
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
        ////Set Panel widths
        //leftPanelParent.preferredWidth = data.leftWidth * baseWidth;
        //rightPanelParent.preferredWidth = data.rightWidth * baseWidth;

        //Initialize Left Panels
        foreach (Transform child in leftPanelParent.transform)
        {
            child.transform.SetParent(hiddenPanelsParent.transform, false);
        }
        for (int i = 0; i < data.leftPanels.Count; i++)
        {
            panels[data.leftPanels[i]].transform.SetParent(leftPanelParent.transform, false);
            panels[data.leftPanels[i]].SetHeight(data.leftHeights[i] * baseHeight);
        }
        foreach (PanelType panel in data.leftPanels) 
        {
        }

        //Initialize Right Panels
        foreach (Transform child in rightPanelParent.transform)
        {
            child.transform.SetParent(hiddenPanelsParent.transform, false);
        }

        for (int i = 0; i < data.rightPanels.Count; i++)
        {
            panels[data.rightPanels[i]].transform.SetParent(rightPanelParent.transform, false);
            panels[data.rightPanels[i]].SetHeight(data.rightHeights[i] * baseHeight);
        }
    }

    public void AddPanel(PanelType type)
    {
        //Add the new panel to the column with less panels
        if(data.rightPanels.Count > data.leftPanels.Count)
        {
            panels[data.leftPanels.Last<PanelType>()].Initialize(true, false, false);
            panels[type].transform.SetParent(leftPanelParent.transform, false);
            data.leftPanels.Add(type);
            data.leftHeights.Add(580);
            panels[type].Initialize(true, false, true);
        }

        else
        {
            panels[data.rightPanels.Last<PanelType>()].Initialize(false, false, false);
            panels[type].transform.SetParent(rightPanelParent.transform, false);
            data.rightPanels.Add(type);
            data.rightHeights.Add(580);
            panels[type].Initialize(false, false, true);
        }

        SavePanelData();
    }

    public void RemovePanel(PanelType type, bool left)
    {
        panels[type].transform.SetParent(hiddenPanelsParent.transform, false);
        if (left)
        {
            data.leftHeights.RemoveAt(data.leftPanels.IndexOf(type));
            data.leftPanels.Remove(type);
        }
        else
        {
            data.rightHeights.RemoveAt(data.rightPanels.IndexOf(type));
            data.rightPanels.Remove(type);
        }

    }

    public bool MoveUp(PanelType type, bool left)
    {
        int index = panels[type].transform.GetSiblingIndex();
                
        if (index == 0)
            return true;

        index -= 1;

        if (left) 
        {
            panels[type].transform.SetSiblingIndex(index);
            data.leftPanels.Remove(type);
            data.leftPanels.Insert(index, type);
        }
        else
        {
            panels[type].transform.SetSiblingIndex(index);
            data.leftPanels.Remove(type);
            data.leftPanels.Insert(index, type);
        }

        if (index == 0)
            return true;
        return false;
    }

    public bool MoveDown(PanelType type, bool left)
    {
        int index = panels[type].transform.GetSiblingIndex();
        
        index += 1;

        if (left)
        {
            if (index > data.leftPanels.Count)
                return true;
            panels[type].transform.SetSiblingIndex(index);
            data.leftPanels.Remove(type);
            data.leftPanels.Insert(index, type);

            if (index > data.leftPanels.Count)
                return true;
        }
        else
        {
            if (index > data.leftPanels.Count)
                return true;
            panels[type].transform.SetSiblingIndex(index);
            data.leftPanels.Remove(type);

            if (index > data.leftPanels.Count)
                return true;
        }
        return false;
    }

    public bool MoveSideways(PanelType type, bool left)
    {
        if (left)
        {
            panels[type].transform.SetParent(rightPanelParent.transform, false);
            return false;
        }
        else
        {
            panels[type].transform.SetParent(leftPanelParent.transform, false);
            return true;
        }
    }

}
