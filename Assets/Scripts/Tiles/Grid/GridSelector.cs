using Newtonsoft.Json;
using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSelector : MonoBehaviour
{

    public static GridSelector Instance;
    [SerializeField] private GameObject selector;
    private string gridmapPath;
    public List<string> gridmapList = new();
    public Transform content;
    public GridButton buttonPrefab;
    public GridMap loadedGridMap;
    public DataNamer gridmapNamer;
    public TMP_Text currentMapText;
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
    }

    private void Start()
    {
        NewGridMap();
    }

    public void Delete(string fileName)
    {
        if (fileName == backupGridMap)
            NewGridMap();

        File.Delete(Path.Combine(gridmapPath, fileName));
        gridmapList.Remove(fileName);
        RefreshSelector();
    }

    public void OpenSelector()
    {
        selector.SetActive(true);
        DMManager.onGrid = false;
    }
    public void CloseSelector()
    {
        selector.SetActive(false);
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
        //If the name already exists, don't allow the new name
        if (gridmapList.Contains(loadedGridMap.mapName))
        {
            gridmapNamer.DuplicateError(loadedGridMap.mapName);
            Debug.Log(loadedGridMap + " is a duplicate");

            return;
        }

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

        loadedGridMap.mapName = new string(backupGridMap);

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
        if (gridmapList.Contains(newName) && newName != backupGridMap)
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
        backupGridMap = new string(loadedGridMap.mapName);
        gridmapNamer.SetName("");
        GridSizer.Instance.LoadSize(10, 10);
        GridRenderer.Instance.LoadGridMap(loadedGridMap);
        CloseSelector();
        saveExisting.interactable = false;
        currentMapText.text = "New Grid Map";
    }

    public void SelectGridMap(string mapName)
    {
        loadedGridMap = JsonConvert.DeserializeObject<GridMap>(File.ReadAllText(Path.Combine(gridmapPath, mapName)));
        backupGridMap = new string(loadedGridMap.mapName);
        currentMapText.text = backupGridMap;
        gridmapNamer.SetName(loadedGridMap.mapName);
        GridRenderer.Instance.LoadGridMap(loadedGridMap);
        CloseSelector();
        saveExisting.interactable = true;
    }
}