using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public enum PanelType
{
    Viewport,
    GridEditor,
    MediaLibrary,
    SFXBoards,
    CombatTracker
}

[System.Serializable]
public class ManagerData
{

    public List<PanelType> leftPanels = new List<PanelType>();
    public List<float> leftHeights = new List<float>();
    public List<PanelType> rightPanels = new List<PanelType>();
    public List<float> rightHeights = new List<float>();

    public float horizontalRatio = .5f;

    public ManagerData() 
    {
        leftPanels.Add(PanelType.Viewport);
        leftHeights.Add(.5f);
        leftPanels.Add(PanelType.GridEditor);
        leftHeights.Add(.5f);
        rightPanels.Add(PanelType.MediaLibrary);
        rightHeights.Add(1);

        horizontalRatio = .5f;
        
    }

    public void UpdateHeight(PanelType target, bool left, float widthDelta)
    {
        if (left)
        {
            foreach(float f in leftHeights)
            {
                if (f < .1f)
                    return;
            }
            int i = leftPanels.IndexOf(target);

            leftHeights[i] += widthDelta;
            leftHeights[i + 1] -= widthDelta;
            
        }
        else
        {
            foreach (float f in rightHeights)
            {
                if (f < .1f)
                    return;
            }
            int i = rightPanels.IndexOf(target);

            rightHeights[i] += widthDelta;
            rightHeights[i + 1] -= widthDelta;
        }
    }

}