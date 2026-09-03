using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class DividerController : MonoBehaviour
{
    private DockPanel topPanel;
    private DockPanel bottomPanel;
    private float splitRatio;

    private bool isDragging;
    private bool isHoveringDivider;
    private Vector2 localPosition;
    private Vector2 mousePosition;

    [SerializeField] private RectTransform rect;

    public DividerController(DockPanel top, DockPanel bottom, float ratio)
    {
        topPanel = top;
        bottomPanel = bottom;
        splitRatio = ratio;

        topPanel.SetMinAnchor(splitRatio);
        bottomPanel.SetMaxAnchor(splitRatio);

        rect.anchorMax = new Vector2(1, splitRatio);
        rect.anchorMin = new Vector2(0, splitRatio);
    }

    public void Initialize(DockPanel top, DockPanel bottom, float ratio)
    {
        topPanel = top;
        bottomPanel = bottom;
        splitRatio = ratio;

        //topPanel.SetMinAnchor(splitRatio);
        //bottomPanel.SetMaxAnchor(splitRatio);

        rect.anchorMax = new Vector2(1, splitRatio);
        rect.anchorMin = new Vector2(0, splitRatio);

    }

    public void UpdateRatio(float ratio)
    {
        splitRatio = ratio;

        topPanel.SetMinAnchor(splitRatio);
        bottomPanel.SetMaxAnchor(splitRatio);

        rect.anchorMax = new Vector2(1, splitRatio);
        rect.anchorMin = new Vector2(0, splitRatio);
    }
    public void SwapPanels()
    {
        DockPanel temp = topPanel;
        topPanel = bottomPanel;
        bottomPanel = temp;

        topPanel.SetMinAnchor(splitRatio);
        bottomPanel.SetMaxAnchor(splitRatio);
    }

    public void ReplaceTopPanel(DockPanel panel)
    {
        topPanel = panel;

        topPanel.SetMinAnchor(splitRatio);
    }

    public void ReplaceBottomPanel(DockPanel panel)
    {
        bottomPanel = panel;

        bottomPanel.SetMaxAnchor(splitRatio);
    }

    public void Update()
    {
        //Click and drag the camera
        if (!isDragging)
        {
            //Check Mouse is in the image
            bool nowHovering;
            //Get the local position and whether we are hovering over the scene image
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Mouse.current.position.ReadValue(), null, out localPosition))
            {
                nowHovering = false;
            }

            nowHovering = rect.rect.Contains(localPosition);

            //If the mouse is not over the rect, don't bother continuing the rest of the updates
            if (!nowHovering)
            {
                if (isHoveringDivider)
                    CursorController.Instance.RevertCursor();
                isHoveringDivider = nowHovering;
                return;
            }

            if (!isHoveringDivider)
                CursorController.Instance.SetCursor(ToolState.VDrag);

            isHoveringDivider = nowHovering;

            //If a click is detecting, start dragging
            if (PanelManager.Instance.leftClick.IsPressed())
            {
                isDragging = true;
                mousePosition = Camera.main.ScreenToViewportPoint(Mouse.current.position.value);
            }
        }
        else
        {
            //If a click is still detected, drag by the mouse movement delta times a dampener value
            if (PanelManager.Instance.leftClick.IsPressed())
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
                if (mousePosition.y > (splitRatio - bottomPanel.height + .1f) && mousePosition.y < (splitRatio + topPanel.height - .1f))
                {
                    UpdateRatio(mousePosition.y);
                }

            }
            //If a click is not detected, stop dragging
            else
            {
                isDragging = false;
            }
        }
    }
}

