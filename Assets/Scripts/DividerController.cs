using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DividerController : MonoBehaviour
{
    private DockPanel topPanel;
    private DockPanel bottomPanel;
    private float splitRatio;

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

        topPanel.SetMinAnchor(splitRatio);
        bottomPanel.SetMaxAnchor(splitRatio);

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
}
