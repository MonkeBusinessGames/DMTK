using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SFB;
using Unity.VisualScripting;
using UnityEngine;
public class PaletteManager : MonoBehaviour
{
    public static PaletteManager Instance;

    //Palette Data Elements
    private string palettesPath;
    public Dictionary<string, PaletteData> palettes = new();
    public Dictionary<int, Sprite> tileSpriteCache = new();
    public Dictionary<int, TileData> tileDataCache = new();
    public Dictionary<int, string> tileLibrary = new();
    public PaletteData loadedPalette = null;
    public PaletteData tempPalette = null;
    public PaletteData backupPalette = null;

    //Palette Selector Elements
    public RectTransform selectorContent;
    public RectTransform managerContent;
    public RectTransform tilePanel;
    public PaletteButton palettePrefab;
    public TilePreview tilePrefab;
    public TileButton tileButtonPrefab;
    [SerializeField] private GameObject paletteSelector;
    [SerializeField] private GameObject paletteManager;
    [SerializeField] private DataNamer paletteNamer;

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects
        Instance = this;

        //Define the palette path
        palettesPath = Path.Combine(Application.persistentDataPath, "Palettes");

        //If the folder for storing palettes doesn't exist, create it.
        if (!Directory.Exists(palettesPath))
            Directory.CreateDirectory(palettesPath);

        if(File.Exists(Path.Combine(palettesPath, "TileLibrary")))
            tileLibrary = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(Path.Combine(palettesPath, "TileLibrary")));


        //Refresh the palettes list
        RefreshPaletteList();
    }

    /// <summary>
    /// Refresh the list of palettes.
    /// </summary>
    public void RefreshPaletteList()
    {
        //Empty the current palettes list
        palettes.Clear();

        //If there are no files, don't do anything.
        if (!Directory.Exists(palettesPath))
            return;

        foreach (var palettePath in Directory.GetDirectories(palettesPath))
        {
                palettes.Add(new DirectoryInfo(palettePath).Name, JsonConvert.DeserializeObject<PaletteData>(File.ReadAllText(Path.Combine(palettePath, "PaletteData"))));
        }

        //Refresh the selector;
        RefreshPaletteSelector();
    }

    /// <summary>
    /// Load all the tiles for a palette palette.
    /// </summary>
    /// <param name="paletteName"></param>
    /// <returns>The file name for the palette to be loaded</returns>
    public void SelectPalette(string paletteName)
    {
        CloseSelector();

        foreach (Transform child in tilePanel)
            Destroy(child.gameObject);

    loadedPalette = JsonConvert.DeserializeObject<PaletteData>(File.ReadAllText(Path.Combine(palettesPath, paletteName, "PaletteData")));

        int i = 0;
        foreach (var tile in loadedPalette.tList)
        {
            byte[] data = File.ReadAllBytes(Path.Combine(loadedPalette.palettePath, tile.Value));
            
            Texture2D tex = new Texture2D(2, 2);
                
            tex.LoadImage(data);
            tex.filterMode = FilterMode.Bilinear;

            tileSpriteCache.Add(tile.Key, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100));
            tileDataCache.Add(tile.Key, JsonConvert.DeserializeObject<TileData>(File.ReadAllText(Path.Combine(loadedPalette.palettePath, Path.GetFileNameWithoutExtension(tile.Value) + "data"))));

            var btn = Instantiate(tileButtonPrefab, tilePanel);
            btn.Setup(tile.Key, i);

            i++;
            //Debug.Log("Loaded" + tile.Value);

            if (!tileLibrary.ContainsKey(tile.Key))
                tileLibrary.Add(tile.Key, Path.Combine(loadedPalette.palettePath, tile.Value));
        }

        //Update the Tile Library json
        File.WriteAllText(Path.Combine(palettesPath, "TileLibrary"), JsonConvert.SerializeObject(tileLibrary));

        //Resize scroll content transform
        tilePanel.sizeDelta = new Vector2(0, 20 + (200 * Mathf.Ceil(i / 2)));
    }

    /// <summary>
    /// Opens the palette selector
    /// </summary>
    public void OpenSelector()
    {
        paletteSelector.SetActive(true);
        DMManager.onGrid = false;
        CursorController.Instance.SetCursor(ToolState.Select);
    }
    
    /// <summary>
    /// Closes the palette selector
    /// </summary>
    public void CloseSelector()
    {
    //    //Clear selector
    //    foreach (Transform child in selectorContent)
    //        Destroy(child.gameObject);

    //    StartCoroutine(WaitOneFrame());

        paletteSelector.SetActive(false);
        DMManager.onGrid = true;
    }
   
    /// <summary>
    /// Refreshes the palettes shown in the selector
    /// </summary>
    public void RefreshPaletteSelector()
    {

        foreach (Transform child in selectorContent)
            Destroy(child.gameObject);

        StartCoroutine(WaitOneFrame());

        int i = 0;
        foreach (var palette in palettes.Values)
        {

            var btn = Instantiate(palettePrefab, selectorContent);

            if (palette.mainSprite == null)
                btn.Setup(palette.paletteName, i);
            else
            {
                //Create the sprite
                byte[] data = File.ReadAllBytes(Path.Combine(palette.palettePath, palette.mainSprite));

                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(data);
                tex.filterMode = FilterMode.Bilinear;

                //Setup the preview object
                btn.Setup(palette.paletteName, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100), i);
            }

            i++;
            //Resize scroll content transform
            selectorContent.sizeDelta = new Vector2(0, 20 + (180 * i));
            Debug.Log("new plist item " + palette.paletteName);
        }
    }

    /// <summary>
    /// Edit an existing palette
    /// </summary>
    /// <param name="paletteName">The name of the palette to edit</param>
    public void EditPalette(string paletteName)
    {

        //Close the palette selector
        UpdateView(true);

        DMManager.onGrid = false;

        //Create a backup of the palette and temporary palette to make changes to.
        string json = File.ReadAllText(Path.Combine(palettesPath, paletteName, "PaletteData"));
        tempPalette = JsonConvert.DeserializeObject<PaletteData>(json);
        backupPalette = JsonConvert.DeserializeObject<PaletteData>(json);

        Debug.Log(tempPalette);
        
        ////Create the new palette folder
        //Directory.CreateDirectory(tempPath);

        ////Copy all files over to the new folder
        //foreach (var fileName in Directory.GetFiles(tempPalette.palettePath))
        //{
        //    string sourcePath = Path.Combine(oldPath, fileName);
        //    string destPath = Path.Combine(tempPath, fileName);
        //    Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
        //    File.Copy(sourcePath, destPath, overwrite: true);
        //}

        //Set the palette name in the UI
        paletteNamer.SetName(paletteName);

        //Refresh the tile list
        RefreshTileList();

    }
    
    /// <summary>
    /// Create a new palette
    /// </summary>
    public void CreatePalette()
    {

        //Close the palette selector
        UpdateView(true);

        DMManager.onGrid = false;

        //Create a temporary and backup palette with no information
        tempPalette = new PaletteData(palettesPath);
        backupPalette = new PaletteData(palettesPath);

        //Create the palette folder
        Directory.CreateDirectory(tempPalette.palettePath);

        //Create the palette data json
        File.WriteAllText(Path.Combine(tempPalette.palettePath, "PaletteData"), JsonConvert.SerializeObject(tempPalette, Formatting.Indented));

        //Set the backup palette to null
        

        //Set the palette name in the UI
        paletteNamer.SetName("");

        //Refresh the tile list
        RefreshTileList();
    }

    //Editing Actions
    /// <summary>
    /// Update the temp palette name
    /// </summary>
    /// <param name="newName">The new name of the temp palette</param>
    public void EditName(string newName)
    {
        //If the name already exists, don't allow the new name
        if (palettes.ContainsKey(newName))
        {
            paletteNamer.DuplicateError(tempPalette.paletteName);
            Debug.Log(newName + " is a duplicate");

            return;
        }
        Debug.Log(tempPalette.paletteName + " is renamed to " + newName);

        paletteNamer.NoError();
        tempPalette.paletteName = newName;
        tempPalette.palettePath = Path.Combine(palettesPath, newName);
    }

    /// <summary>
    /// Allow users to add a new tile to a palette.
    /// </summary>
    public void ImportTiles()
    {
        var Paths = StandaloneFileBrowser.OpenFilePanel("Import palette", "", new[] { new ExtensionFilter("Images", "png", "jpg", "webp") }, true);

        if (Paths.Length == 0) return;

        foreach (var sourcePath in Paths)
        {
            //Copy the file over to the palette's folder
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(backupPalette.palettePath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);

            //Generate a unique ID for the tile 
            int tempID = UnityEngine.Random.Range(0, 99999);
            while (tileLibrary.ContainsKey(tempID))
                tempID = UnityEngine.Random.Range(0, 99999);

            //Create the tileData and add it to the paletteData
            File.WriteAllText(Path.Combine(backupPalette.palettePath, Path.GetFileNameWithoutExtension(fileName) + "data"), JsonConvert.SerializeObject(new TileData(fileName, tempID)));
            tempPalette.tList[tempID] = fileName;
        }

        //Refresh the tile list
        RefreshTileList();
    }

    /// <summary>
    /// Update a new Palette
    /// </summary>
    /// <param name="data">The palette data to update</param>
    public void UpdatePalette()
    {
        //If the name is empty, don't let them save
        if (!paletteNamer.RequiredCheck())
            return;

        Debug.Log("Continues Save");

        //Close the palette manager
        UpdateView(false);

        //If you're editing an existing palette, delete the removed tiles
        if (backupPalette != null)
        {
            //Check all backed up tiles to confirm whether they should be kept
            foreach (var tile in backupPalette.tList.Values)
            {
                //If a tile is not longer found in the tlist, delete the image and image data.
                if (!tempPalette.tList.ContainsValue(tile))
                {
                    File.Delete(Path.Combine(backupPalette.palettePath, tile));
                    File.Delete(Path.Combine(backupPalette.palettePath, Path.GetFileNameWithoutExtension(tile) + "data"));
                }
            }

            //Rename the palette folder if the name changed from the existing palette
            if (backupPalette.paletteName != tempPalette.paletteName)
                Directory.Move(backupPalette.palettePath, tempPalette.palettePath);
        }
        //If you're editing a new palette, you always need to rename the folder
        else
            Directory.Move(backupPalette.palettePath, tempPalette.palettePath);

        //Make sure the tileLibrary is updated with all the tiles and their paths.
        foreach(var tile in tempPalette.tList)
        {
                tileLibrary[tile.Key] = Path.Combine(tempPalette.palettePath, tile.Value);
        }

        //Create or Update the palette data json
        File.WriteAllText(Path.Combine(tempPalette.palettePath, "PaletteData"), JsonConvert.SerializeObject(tempPalette, Formatting.Indented));
        Debug.Log(tempPalette);

        //Update the Tile Library json
        File.WriteAllText(Path.Combine(palettesPath, "TileLibrary"), JsonConvert.SerializeObject(tileLibrary));

        //Refresh the selector;
        RefreshPaletteList();
    }

    /// <summary>
    /// Cancel a palette create or edit
    /// </summary>
    public void CancelUpdate()
    {
        //Close the palette manager
        UpdateView(false);

        //Check all temp tiles to confirm whether they should be kept
        foreach (var tile in tempPalette.tList.Values)
        {
            //If a tile is not found in the backup palette, delete it.
            if (!backupPalette.tList.ContainsValue(tile))
            {
                File.Delete(Path.Combine(backupPalette.palettePath, tile));
                File.Delete(Path.Combine(backupPalette.palettePath, Path.GetFileNameWithoutExtension(tile) + "data"));
            }
        }

        //Refresh the Palette Selector
        RefreshPaletteSelector();
    }

    /// <summary>
    /// Delete a tile within a palette
    /// </summary>
    /// <param name="fileName">The tile to be deleted.</param>
    public void DeleteTile(int tileKey)
    {
        //Remove the tile reference from the palette data
        tempPalette.tList.Remove(tileKey);
        //Refresh the tile list
        RefreshTileList();
    }

    /// <summary>
    /// Delete an entire palette
    /// </summary>
    /// <param name="palette">The palette to be deleted.</param>
    public void DeletePalette()
    {
        //Close the palette manager
        UpdateView(false);

        //Delete the old folder
        Directory.Delete(backupPalette.palettePath, true);

        //Remove the palette from the palette list
        palettes.Remove(backupPalette.paletteName);

        //Refresh the selector;
        RefreshPaletteSelector();

    }

    /// <summary>
    /// Refresh the list of tiles shown in the palette manager
    /// </summary>
    public void RefreshTileList()
    {
        //Destroy the existing tiles in the list
        foreach (Transform child in managerContent)
            Destroy(child.gameObject);

        StartCoroutine(WaitOneFrame());
        int i = 0;

        //If the preview sprite is no longer in the palette, get a new one.
        bool resetPreview = false;
        try
        {
            if(!tempPalette.tList.ContainsValue(tempPalette.mainSprite))
                resetPreview = true;
        }
        catch (ArgumentNullException)
        {
            resetPreview = true;
        }

        foreach (var tile in tempPalette.tList)
        {
            //Set the new preview sprite as the first sprite 
            if (resetPreview)
            {
                tempPalette.mainSprite = tile.Value;
                resetPreview = false;
            }

            //Instantiate the preview object
            var btn = Instantiate(tilePrefab, managerContent);

            //Create the sprite
            byte[] data = File.ReadAllBytes(Path.Combine(backupPalette.palettePath, tile.Value));

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(data);
            tex.filterMode = FilterMode.Bilinear;
            
            //Setup the preview object
            btn.Setup(tile.Key, tile.Value, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100), i);

            i++;
            Debug.Log("new list item " + tile);
        }

        //Resize scroll content transform
        managerContent.sizeDelta = new Vector2(0, 20 + 400 * Mathf.Ceil((float)i / 5));

        //Set the null as the preview sprite if there are no more sprites.
        if (resetPreview)
            tempPalette.mainSprite = null;
        

    }

    public void UpdateView(bool managingPalette) 
    {
        if (managingPalette)
        {
            //Open the manage view only
            paletteManager.SetActive(true);
            paletteSelector.SetActive(false);

            //Clear selector
            foreach (Transform child in selectorContent)
                Destroy(child.gameObject);
        }
        else
        {
            //Open the select view only
            paletteManager.SetActive(false);
            paletteSelector.SetActive(true);

            //Clear manager
            foreach (Transform child in managerContent)
            {
                Destroy(child.gameObject);
            }
        }

        StartCoroutine(WaitOneFrame());
    }

    private IEnumerator WaitOneFrame()
    {
        yield return null;

        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public Sprite GetTileSprite(int tileID)
    {
        if(tileSpriteCache.TryGetValue(tileID, out var sprite))
        {
            //Debug.Log(tileID + " found in cache");
            return sprite;
        }

        if (tileLibrary.TryGetValue(tileID, out var path))
        {
            byte[] data = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);

            tex.LoadImage(data);
            tex.filterMode = FilterMode.Bilinear;

            tileSpriteCache.Add(tileID, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100));
            tileDataCache.Add(tileID, JsonConvert.DeserializeObject<TileData>(File.ReadAllText((path.TrimEnd(Path.GetExtension(path)) + "data"))));

            Debug.Log(tileID + " added to cache from " + path);
            return tileSpriteCache[tileID];
        }

        Debug.Log(tileID + " not found in cache or library");
        return null;

    }
}
