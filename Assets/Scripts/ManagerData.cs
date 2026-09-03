using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Rendering;

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

    public void Initialize()
    {
        leftPanels.Add(PanelType.Viewport);
        leftHeights.Add(.5f);
        leftHeights.Add(.5f);
        leftPanels.Add(PanelType.GridEditor);

        rightPanels.Add(PanelType.MediaLibrary);
        rightHeights.Add(1f);

        horizontalRatio = .5f;
    }

    public void AddPanel(PanelType panelType, bool left)
    {
        //Check whether left or right
        if (left)
        {
            //Add the panel
            leftPanels.Add(panelType);

            if (leftPanels.Count > 1)
            {
                //Calculate the height changes needed for the rest of the panels (new height defaulted to .3f)
                float heightDif = .3f / (leftHeights.Count);
                for (int i = 0; i < leftHeights.Count; i++)
                    leftHeights[i] -= heightDif;

                //Add the new height
                leftHeights.Add(.3f);
            }
            else
                //Add the new height
                leftHeights.Add(1f);
        }
        else
        {
            //Add the panel
            rightPanels.Add(panelType);

            if (rightPanels.Count > 1)
            {
                //Calculate the height changes needed for the rest of the panels (new height defaulted to .3f)
                float heightDif = .3f / (rightHeights.Count);
                for (int i = 0; i < rightHeights.Count; i++)
                    rightHeights[i] -= heightDif;

                //Add the new height
                rightHeights.Add(.3f);
            }
            else
                //Add the new height
                rightHeights.Add(1);
        }
    }

    public void RemovePanel(PanelType panelType, bool left)
    {
        //Check whether left or right
        if (left)
        {
            //Calculate the height changes needed for the rest of the panels
            float heightDif = leftHeights[leftPanels.IndexOf(panelType)] / (leftHeights.Count - 1);

            //Remove the corresponding height
            leftHeights.RemoveAt(leftPanels.IndexOf(panelType));

            //Remove the panel
            leftPanels.Remove(panelType);

            //Update the heights of the rest of the panels to take up the space of the removed panel
            for (int i = 0; i < leftHeights.Count; i++)
                leftHeights[i] += heightDif;

        }
        else
        {
            //Calculate the height changes needed for the rest of the panels
            float heightDif = rightHeights[rightPanels.IndexOf(panelType)] / (rightHeights.Count - 1);

            //Remove the corresponding height
            rightHeights.RemoveAt(rightPanels.IndexOf(panelType));

            //Remove the panel
            rightPanels.Remove(panelType);


            //Update the heights of the rest of the panels to take up the space of the removed panel
            for (int i = 0; i < rightHeights.Count; i++)
                rightHeights[i] += heightDif;

        }
    }

    public void MovePanelUp(PanelType panelType, bool left)
    {
        //Check whether left or right
        if (left)
        {
            int index = leftPanels.IndexOf(panelType);
            leftHeights.TrySwap(index, index - 1, out Exception heightException);
            leftPanels.TrySwap(index, index - 1, out Exception panelException);

        }
        else
        {

            int index = rightPanels.IndexOf(panelType);
            rightHeights.TrySwap(index, index - 1, out Exception heightException);
            rightPanels.TrySwap(index, index - 1, out Exception panelException);

        }
    }

    public void MovePanelDown(PanelType panelType, bool left)
    {
        //Check whether left or right
        if (left)
        {
            int index = leftPanels.IndexOf(panelType);
            leftHeights.TrySwap(index, index + 1, out Exception heightException);
            leftPanels.TrySwap(index, index + 1, out Exception panelException);

        }
        else
        {

            int index = rightPanels.IndexOf(panelType);
            rightHeights.TrySwap(index, index + 1, out Exception heightException);
            rightPanels.TrySwap(index, index + 1, out Exception panelException);
        }
    }
}