using Newtonsoft.Json;
using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GridSelector : MonoBehaviour
{

    public static GridSelector Instance;
    private string gridmapPath;
    public List<string> gridmapList = new();
    public Transform content;
    public GridButton buttonPrefab;
    public GridMap loadedGridMap;
    public DataNamer gridmapNamer;
    public string backupGridMap;
    public Button saveExisting;

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

        //Define the gridmap path
        gridmapPath = Path.Combine(Application.persistentDataPath, "GridMaps");

        //If the folder for storing gridmaps doesn't exist, create it.
        if (!Directory.Exists(gridmapPath))
            Directory.CreateDirectory(gridmapPath);

        //Set the GridList
        foreach(var file in Directory.GetFiles(gridmapPath))
        {
            gridmapList.Add(Path.GetFileName(file));
        }

        //Refresh the gridmaps selector list
        RefreshSelector();

        gridmapNamer.SetName(loadedGridMap.mapName);

        NewGridMap();
    }

    public void Delete(string fileName)
    {
        File.Delete(Path.Combine(gridmapPath, fileName));
        gridmapList.Remove(fileName);
        RefreshSelector();
    }

    public void OpenSelector()
    {
        gameObject.SetActive(true);
        DMManager.onGrid = false;
    }
    public void CloseSelector()
    {
        gameObject.SetActive(false);
        DMManager.onGrid = true;
    }

    public void RefreshSelector()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var bg in gridmapList)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(bg, i);
            i++;
            Debug.Log("new list item " + bg);   
        }
    }

    public void SaveNewGridMap()
    {
        //If the name is empty, don't let them save
        if (!gridmapNamer.RequiredCheck())
            return;

        gridmapList.Add(loadedGridMap.mapName);
        File.WriteAllText(Path.Combine(gridmapPath, loadedGridMap.mapName), JsonConvert.SerializeObject(loadedGridMap));

        RefreshSelector();
    }

    public void SaveExistingGridMap()
    {
        //If the name is empty, don't let them save
        if (!gridmapNamer.RequiredCheck())
            return;

        gridmapList.Add(loadedGridMap.mapName);
        Delete(backupGridMap);
        File.WriteAllText(Path.Combine(gridmapPath, loadedGridMap.mapName), JsonConvert.SerializeObject(loadedGridMap));

        RefreshSelector();
    }

    /// <summary>
    /// Update the gridmap name
    /// </summary>
    /// <param name="newName">The new name of the gridmap</param>
    public void EditName(string newName)
    {
        //If the name already exists, don't allow the new name
        if (gridmapList.Contains(newName))
        {
            gridmapNamer.DuplicateError(loadedGridMap.mapName);
            Debug.Log(newName + " is a duplicate");

            return;
        }
        Debug.Log(loadedGridMap.mapName + " is renamed to " + newName);

        gridmapNamer.NoError();
        loadedGridMap.mapName = newName;
    }

    public void NewGridMap()
    {
        loadedGridMap = new GridMap("", 10, 10);
        GridRenderer.Instance.LoadGridMap();
        CloseSelector();
        saveExisting.interactable = false;
    }

    public void SelectGridMap(string mapName)
    {
        loadedGridMap = JsonConvert.DeserializeObject<GridMap>(File.ReadAllText(Path.Combine(gridmapPath, mapName)));
        gridmapNamer.SetName(loadedGridMap.mapName);
        GridRenderer.Instance.LoadGridMap();
        CloseSelector();
        saveExisting.interactable = true;
    }
}