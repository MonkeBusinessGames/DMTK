using UnityEngine;
using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PaletteManager : MonoBehaviour
{
    public static PaletteManager Instance;

    //Palette Data Elements
    private string palettesPath;
    public List<string> palettes = new();
    public Dictionary<string, Sprite> loadedTiles = new();
    public PaletteData loadedPalette = null;
    public PaletteData tempPalette = null;
    private string editedPaletteName = null;
    private string tempPath;

    //Palette Selector Elements
    public Transform selectorContent;
    public Transform managerContent;
    public PaletteButton palettePrefab;
    public TilePreview tilePrefab;
    [SerializeField] private GameObject paletteSelector;
    [SerializeField] private GameObject paletteManager;
    [SerializeField] private PaletteNamer paletteNamer;

    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects and don't destory it.
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Define the palette path
        palettesPath = Path.Combine(Application.persistentDataPath, "Palettes");
        tempPath = Path.Combine(palettesPath, "temp");

        //If the folder for storing palettes doesn't exist, create it.
        if (!Directory.Exists(palettesPath))
            Directory.CreateDirectory(palettesPath);

        palettes = new List<string>(Directory.GetFileSystemEntries(palettesPath));

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

        foreach (var file in Directory.GetFiles(palettesPath))
        {
            palettes.Add(new string(Path.GetFileName(file)));
            Debug.Log(Path.GetFileName(file));
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
        
        loadedPalette = JsonUtility.FromJson<PaletteData>(File.ReadAllText(Path.Combine(palettesPath, paletteName, "PaletteData")));

        Texture2D tex = new Texture2D(2, 2);
        foreach (var tile in loadedPalette.tList.Keys)
        {
            byte[] data = File.ReadAllBytes(Path.Combine(loadedPalette.palettePath, tile));

            tex.LoadImage(data);
            tex.filterMode = FilterMode.Bilinear;

            loadedTiles.Add(tile, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100));
        }

        CloseSelector();
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
        paletteSelector.SetActive(false);
        DMManager.onGrid = true;
    }
   
    /// <summary>
    /// Refreshes the palettes shown in the selector
    /// </summary>
    public void RefreshPaletteSelector()
    {
        int i = 0;
        foreach (var palette in palettes)
        {
            var btn = Instantiate(palettePrefab, selectorContent);
            btn.Setup(palette, i);
            i++;
            Debug.Log("new list item " + palette);
        }
    }

    /// <summary>
    /// Edit an existing palette
    /// </summary>
    /// <param name="paletteName">The name of the palette to edit</param>
    public void EditPalette(string paletteName)
    {
        paletteManager.SetActive(true);
        paletteSelector.SetActive(false);
        DMManager.onGrid = false;

        editedPaletteName = paletteName;

        //Create a temporary palette with the same as the old palette
        tempPalette = JsonUtility.FromJson<PaletteData>(File.ReadAllText(Path.Combine(palettesPath, paletteName, "PaletteData")));

        string oldPath = tempPalette.palettePath;

        //Create the new palette folder
        Directory.CreateDirectory(tempPath);

        //Copy all files over to the new folder
        foreach (var fileName in Directory.GetFiles(tempPalette.palettePath))
        {
            string sourcePath = Path.Combine(oldPath, fileName);
            string destPath = Path.Combine(tempPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

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
        //Open the palette manager
        paletteManager.SetActive(true);
        paletteSelector.SetActive(false);
        DMManager.onGrid = false;

        //Create a temporary palette with no information
        tempPalette = new PaletteData("temp", new Dictionary<string, TileData>(), null);

        //Create the palette folder
        Directory.CreateDirectory(tempPath);

        //Create the palette data json
        File.WriteAllText(Path.Combine(tempPalette.palettePath, "PaletteData"), JsonUtility.ToJson(tempPalette));

        //Set the editable palette to null
        editedPaletteName = "";
    }

    //Editing Actions
    /// <summary>
    /// Update the temp palette name
    /// </summary>
    /// <param name="newName">The new name of the temp palette</param>
    public void EditName(string newName)
    {
        //If the name already exists, don't allow the new name
        if (palettes.Contains(newName))
        {
            paletteNamer.DuplicateError();
            return;
        }

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
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(tempPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
            tempPalette.tList.Add(fileName, new TileData(fileName));
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

        //If you're editing an existing palette, delete the old palette and rename the new one.
        if (palettes.Contains(editedPaletteName))
        {

            //If the names changed, update the names in the palette list
            if(editedPaletteName != tempPalette.paletteName)
            {
                palettes.Remove(editedPaletteName);
                palettes.Add(tempPalette.paletteName);
            }

            //Delete the old folder
            File.Delete(Path.Combine(palettesPath, editedPaletteName));

            //Rename the new folder
            Directory.Move(tempPath, tempPalette.palettePath);

            //Update the palette data json
            File.WriteAllText(Path.Combine(tempPalette.palettePath, "PaletteData"), JsonUtility.ToJson(tempPalette));
        }
        else
        {
            //Rename the new folder
            Directory.Move(tempPath, tempPalette.palettePath);
            
            //Create the palette data json
            File.WriteAllText(Path.Combine(tempPalette.palettePath, "PaletteData"), JsonUtility.ToJson(tempPalette));

            //Add the palette to the palette list
            palettes.Add(tempPalette.paletteName);
        }

        //Refresh the selector;
        RefreshPaletteSelector();


        //Close the palette manager
        paletteManager.SetActive(false);
        paletteSelector.SetActive(true);
    }

    /// <summary>
    /// Cancel a palette create or edit
    /// </summary>
    public void CancelUpdate()
    {
        //Delete the temp data
        File.Delete(tempPath);

        //Close the palette manager
        paletteManager.SetActive(false);
        paletteSelector.SetActive(true);
    }

    /// <summary>
    /// Delete a tile within a palette
    /// </summary>
    /// <param name="fileName">The tile to be deleted.</param>
    public void DeleteTile(string tileName)
    {
        //Delete the tile data
        File.Delete(Path.Combine(tempPath, tileName));

        //Remove the tile reference from the palette data
        tempPalette.tList.Remove(tileName);

        //Refresh the tile list
        RefreshTileList();

        Debug.Log("Deleted " + tileName);
    }

    /// <summary>
    /// Delete an entire palette
    /// </summary>
    /// <param name="palette">The palette to be deleted.</param>
    public void DeletePalette(string palette)
    {
        //Delete the old folder
        File.Delete(Path.Combine(palettesPath, editedPaletteName));

        //Delete the temp data
        File.Delete(tempPath);

        //Remove the palette from the palette list
        palettes.Remove(palette);

        //Refresh the selector;
        RefreshPaletteSelector();

        //Close the palette manager
        paletteManager.SetActive(false);
        paletteSelector.SetActive(true);
    }

    /// <summary>
    /// Refresh the list of tiles shown in the palette manager
    /// </summary>
    public void RefreshTileList()
    {
        int i = 0;
        Texture2D tex = new Texture2D(2, 2);
        foreach (var tile in tempPalette.tList.Keys)
        {
            //Instantiate the preview object
            var btn = Instantiate(tilePrefab, managerContent);

            //Create the sprite
            byte[] data = File.ReadAllBytes(Path.Combine(tempPath, tile));

            tex.LoadImage(data);
            tex.filterMode = FilterMode.Bilinear;

            //Setup the preview object
            btn.Setup(tile, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100), i);

            i++;
            Debug.Log("new list item " + tile);
        }
    }
}
