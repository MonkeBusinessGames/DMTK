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
    public PaletteData editablePalette = null;
    private string editedPaletteName = null;

    //Palette Selector Elements
    public Transform content;
    public PaletteButton buttonPrefab;
    [SerializeField] private GameObject paletteSelector;
    [SerializeField] private GameObject paletteManager;

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

        //If the folder for storing palettes doesn't exist, create it.
        if (!Directory.Exists(palettesPath))
            Directory.CreateDirectory(palettesPath);

        palettes = new List<string>(Directory.GetFileSystemEntries(palettesPath));

        //Refresh the palettes list
        RefreshPaletteList();
    }

    /// <summary>
    /// Allow users to add a new tile to a palette.
    /// </summary>
    public void ImportTiles(PaletteData palette)
    {
        var Paths = StandaloneFileBrowser.OpenFilePanel("Import palette", "", new[] { new ExtensionFilter("Images", "png", "jpg", "webp") }, true);

        if (Paths.Length == 0) return;

        foreach (var sourcePath in Paths)
        {
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(palette.palettePath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

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
    /// Delete a tile within a palette
    /// </summary>
    /// <param name="fileName">The tile to be deleted.</param>
    public void DeleteTile(string tileName, PaletteData palette)
    {
        //Delete the tile data
        File.Delete(Path.Combine(palette.palettePath, tileName));

        //Remove the tile reference from the palette data
        palette.tList.Remove(tileName);

        //Save the palette data
        File.WriteAllText(Path.Combine(palette.palettePath, "PaletteData"), JsonUtility.ToJson(palette));
    }

    /// <summary>
    /// Delete an entire palette
    /// </summary>
    /// <param name="palette">The palette to be deleted.</param>
    public void DeletePalette(PaletteData palette)
    {
        //Delete the palette data
        File.Delete(palette.palettePath);

        //Remove the palette from the palette list
        palettes.Remove(palette.name);

        //Refresh the selector;
        RefreshPaletteSelector();
    }

    /// <summary>
    /// Create a temporary Palette for editing or creation purposes
    /// </summary>
    public void TempPalette()
    {
        //Create the palette folder
        Directory.CreateDirectory(editablePalette.palettePath);

        //Create the palette data json
        File.WriteAllText(Path.Combine(editablePalette.palettePath, "PaletteData"), JsonUtility.ToJson(editablePalette));
    }

    /// <summary>
    /// Update a new Palette
    /// </summary>
    /// <param name="data">The palette data to update</param>
    public void UpdatePalette(PaletteData data)
    {
        //Create the palette folder
        Directory.CreateDirectory(data.palettePath);

        //Create the palette data json
        File.WriteAllText(Path.Combine(data.palettePath, "PaletteData"), JsonUtility.ToJson(data));

        //Add the pallete to the palette list
        if(data.paletteName != editedPaletteName)
        palettes.Add(data.name);

        //Refresh the selector;
        RefreshPaletteSelector();
    }

    /// <summary>
    /// Opens the palette selector
    /// </summary>
    public void OpenSelector()
    {
        paletteSelector.SetActive(true);
        DMManager.onGrid = false;
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
        foreach (Transform child in content)
        {
            if (child.name != "Create New")
                Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var palette in palettes)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(palette, i);
            i++;
            Debug.Log("new list item " + palette);
        }
    }

    public void EditPalette(string paletteName)
    {
        paletteManager.SetActive(true);
        DMManager.onGrid = false;
    }

    public void CreatePalette(string paletteName)
    {
        paletteManager.SetActive(true);
        DMManager.onGrid = false;
    }
}
