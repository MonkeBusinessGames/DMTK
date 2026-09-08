using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DockPanel : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject closeButton;

    [SerializeField] private PanelType type;

    public int panelID { get; set; }
    public bool panelLeft { get; set; }

    public void OnOpened() { }

    public void OnClosed() { }

    public void OnFocused() { }

    public void OnUnfocused() { }

    public int position;
    public float height;
    public static int leftCount;
    public static int rightCount;

    public void ResetRect()
    {
        rect.localPosition = Vector3.zero;
        rect.anchoredPosition = Vector3.zero;
        SetMaxAnchor(1);
        SetMinAnchor(0);
        height = 1;
//        Debug.Log(type + "Panel Reset: " + rect);
    }

    public void SetMaxAnchor(float ratio)
    {
        rect.anchorMax = new Vector2(1, ratio);
        height = ratio - rect.anchorMin.y;
        if (panelLeft)
            PanelManager.Instance.data.leftHeights[PanelManager.Instance.data.leftPanels.IndexOf(type)] = height;
        else
            PanelManager.Instance.data.rightHeights[PanelManager.Instance.data.rightPanels.IndexOf(type)] = height;
    }
    public void SetMinAnchor(float ratio)
    {
        rect.anchorMin = new Vector2(0, ratio); 
        height = rect.anchorMax.y - ratio;
        if (panelLeft)
            PanelManager.Instance.data.leftHeights[PanelManager.Instance.data.leftPanels.IndexOf(type)] = height;
        else
            PanelManager.Instance.data.rightHeights[PanelManager.Instance.data.rightPanels.IndexOf(type)] = height;

    }

    public void MoveUp()
    {
        if (position == 0)
            return;
        
        PanelManager.Instance.MoveUp(type, panelLeft) ;

    }

    public void MoveDown()
    {
        if (panelLeft)
        {
            if (position >= leftCount - 1)
                return;
        }
        else
        {
            if (position >= rightCount - 1)
                return;
        }

        PanelManager.Instance.MoveDown(type, panelLeft);
    }
    public void MoveSideWays()
    {
        PanelManager.Instance.MoveSideways(type, panelLeft);
    }
    public void Close()
    {
        PanelManager.Instance.RemovePanel(type, panelLeft);
    }

    public void Initialize(bool left, int pos, float top, float bottom)
    {
        panelLeft = left;
        position = pos;

        rect.anchorMax = new Vector2(1, top);
        rect.anchorMin = new Vector2(0, bottom);

        height = top - bottom;
        
        //The down button is visible if the panel is not at the bottom
        if (panelLeft)
            downButton.SetActive(position != leftCount - 1);
        else
            downButton.SetActive(position != rightCount - 1);

        //The up button is visible if the panel is not at the top
        upButton.SetActive(position != 0);

        //The left buttom is visible if the panel is on the right
        leftButton.SetActive(!panelLeft);

        //The right buttom is visible if the panel is on the left
        rightButton.SetActive(panelLeft);

        Debug.Log(type + " initialized. Left: " + left + " Pos: " + pos + " Anchors " + top + ", " + bottom);
    }

    //public void ChangePosition(int pos, float top, float bottom)
    //{
    //    position = pos;

    //    SetMinAnchor(top);
    //    SetMaxAnchor(bottom);

    //    height = top - bottom;

    //    //The down button is visible if the panel is not at the bottom
    //    if (panelLeft)
    //        downButton.SetActive(position != leftCount - 1);
    //    else
    //        downButton.SetActive(position != rightCount - 1);

    //    //The up button is visible if the panel is not at the top
    //    upButton.SetActive(position != 0);

    //    //The left buttom is visible if the panel is on the right
    //    leftButton.SetActive(!panelLeft);

    //    //The right buttom is visible if the panel is on the left
    //    rightButton.SetActive(panelLeft);
    //}


}
