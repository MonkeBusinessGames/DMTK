using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class LayerSelector : MonoBehaviour
{

    public static LayerSelector Instance;
    [SerializeField] private GameObject selector;
    [SerializeField] private GameObject newLayerPopUp;
    [SerializeField] private DataNamer layerNamer;
    private string backUpName;
    private string newLayerName;
    [SerializeField] private GameObject deletePopUp;
    [SerializeField] private TMP_Text deleteName;
    public List<string> layerList = new();
    public RectTransform content;
    public LayerButton buttonPrefab;
    private bool layersUpdated;

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

        //Refresh the layers selector list
        RefreshSelector();

    }

    public void LoadLayerList(List<TileLayer> gridLayerList)
    {
        layerList = new List<string>();

        foreach(TileLayer layer in gridLayerList)
        {
            layerList.Add(layer.layerName);
        }

        RefreshSelector();
    }

    public void Delete(string layerName)
    {
        //Prevent deletion if there is only one layer
        if (layerList.Count == 1)
        {
            return;
        }

        backUpName = layerName;
        deleteName.text = "Are you sure you want to delete " +backUpName + "? This action cannot be taken back.";
        deletePopUp.SetActive(true);
    }

    public void ConfirmDelete()
    {
        //Remove layer from loaded gridmap
        layerList.Remove(backUpName);

        //Close the confirmation pop-up
        deletePopUp.SetActive(false);

        RefreshSelector();
    }

    public void CancelDelete()
    {
        //Close the confirmation pop-up
        deletePopUp.SetActive(false);
    }

    public void OpenSelector()
    {
        RefreshSelector();

        layersUpdated = false;
        
        //Open the selector
        selector.SetActive(true);
    }
    public void CloseSelector()
    {
        //Close the selector
        selector.SetActive(false);

        //Refresh the Gridmap if changes were made
        if(layersUpdated)
            GridSelector.Instance.RefreshGridMap();

    }

    public void RefreshSelector()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var layer in GridSelector.Instance.loadedGridMap.tileLayers )
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(layer.layerName, i, layer.hide);
            i++;
            //Debug.Log("new list item " + layer);
        }

        //Resize scroll content transform
        content.sizeDelta = new Vector2(0, 20 + (120 * i));

        layersUpdated = true;
    }

    public void AddLayer()
    {
        newLayerPopUp.SetActive(true);
        backUpName = newLayerName = "";
        layerNamer.SetName(backUpName);
    }

    public void EditLayerName(string layerName)
    {
        newLayerPopUp.SetActive(true);
        backUpName = newLayerName = layerName;
        layerNamer.SetName(backUpName);
    }

    public void ConfirmLayerAdd()
    {
        //If the name is empty, don't let them save
        if (!layerNamer.RequiredCheck())
            return;

        //Close the layer namer
        newLayerPopUp.SetActive(false);

        //If this is a new layer, add the layer
        if(backUpName == "")
        {
            layerList.Add(newLayerName);

            //Update the loaded GridMap
            GridSelector.Instance.loadedGridMap.AddLayer(newLayerName);
        }
        //If this is an existing layer, update the layer name
        else
        {        
            //Update the layer at the index of the old name
            layerList[layerList.IndexOf(backUpName)] = newLayerName;

            //Update the loaded GridMap
            GridSelector.Instance.loadedGridMap.UpdateLayerName(newLayerName, layerList.IndexOf(newLayerName));
        }

        RefreshSelector();
    }

    public void EditName(string newName)
    {
        //If the name already exists, don't allow the new name
        if (layerList.Contains(newName) && newName != backUpName)
        {
            layerNamer.DuplicateError(backUpName);

            return;
        }

        layerNamer.NoError();

        newLayerName = newName;
    }

    public void CancelNameEdit()
    {
        //Close the layer namer
        newLayerPopUp.SetActive(false);

        layerNamer.NoError();
    }

    public void MoveLayerUp(string layerName)
    {
        //Get in current index of the layer
        int i = layerList.IndexOf(layerName);
        
        //If layer is already at the top of the list, do nothing
        if ((i == 0))
            return;
        
        //Remove the layer and place it again on place up on the selector list
        layerList.RemoveAt(i);
        layerList.Insert(i-1, layerName);

        //Update the loaded GridMap
        GridSelector.Instance.loadedGridMap.MoveLayerUp(i);

        //Refresh the selector
        RefreshSelector();
    }

    public void MoveLayerDown(string layerName)
    {

        //Get in current index of the layer
        int i = layerList.IndexOf(layerName);

        //If layer is already at the bottom of the list, do nothing
        if ((i == layerList.Count - 1))
            return;

        //Remove the layer and place it again on place up on the list
        layerList.RemoveAt(i);
        layerList.Insert(i + 1, layerName);

        //Update the loaded GridMap
        GridSelector.Instance.loadedGridMap.MoveLayerDown(i);

        //Refresh the selector
        RefreshSelector();
    }

    
    public void HideLayer(string layerName)
    {
        //Update the loaded GridMap
        layersUpdated = true;
        GridSelector.Instance.loadedGridMap.HideLayer(layerList.IndexOf(layerName));

    }

    public void ShowLayer(string layerName)
    {
        //Update the loaded GridMap
        layersUpdated = true;
        GridSelector.Instance.loadedGridMap.ShowLayer(layerList.IndexOf(layerName));

    }

}
