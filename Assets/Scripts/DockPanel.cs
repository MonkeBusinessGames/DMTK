using System;
using UnityEngine;
using UnityEngine.UI;

public class DockPanel : MonoBehaviour
{
    [SerializeField] private LayoutElement layout;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private PanelType type;

    public int panelID { get; set; }
    public bool panelLeft { get; set; }

    public void OnOpened() { }

    public void OnClosed() { }

    public void OnFocused() { }

    public void OnUnfocused() { }

    public void SetHeight(float height)
    {
        layout.preferredHeight = height;
    }

    public void MoveUp()
    {
        if(PanelManager.Instance.MoveUp(type, panelLeft))
        {
            upButton.enabled = false;
            downButton.enabled = true;
        }
        else
        {
            upButton.enabled = true;
            downButton.enabled = false;
        }
    }

    public void MoveDown()
    {
        if (PanelManager.Instance.MoveDown(type, panelLeft))
        {
            upButton.enabled = true;
            downButton.enabled = false;
        }
        else
        {
            upButton.enabled = false;
            downButton.enabled = true;
        }
    }
    public void MoveSideWays()
    {
        PanelManager.Instance.MoveSideways(type, panelLeft);
        if (panelLeft)
        {
            panelLeft = false;
            rightButton.enabled = false;
            leftButton.enabled = true;
        }
        else
        {
            panelLeft = true;
            rightButton.enabled = true;
            leftButton.enabled = false;
        }
    }
    public void Close()
    {
        PanelManager.Instance.RemovePanel(type, panelLeft);
    }

    public void Initialize(bool left, bool top, bool bottom)
    {
        panelLeft = left;
        if (left)
            leftButton.enabled = false;
        else
            rightButton.enabled = false;
        if (top)
            upButton.enabled = false;
        if(bottom)
            downButton.enabled = false;
    }
}
