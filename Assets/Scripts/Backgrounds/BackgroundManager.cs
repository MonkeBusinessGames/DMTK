using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SFB;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;
    [SerializeField] BackgroundList bList;
    private string backgroundsPath;
    public List<string> backgrounds = new();

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

        //Define the background path
        backgroundsPath = Path.Combine(Application.persistentDataPath, "Backgrounds");

        //If the folder for storing backgrounds doesn't exist, create it.
        if (!Directory.Exists(backgroundsPath))
        {
            Directory.CreateDirectory(backgroundsPath);
            InitializeDefaultContent();
        }



        //Refresh the backgrounds list
        Refresh();

    }

    private void InitializeDefaultContent()
    {
        string[] paths = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Backgrounds"));

        if (paths.Length == 0) return;

        foreach (var sourcePath in paths)
        {
            string fileName = Path.GetFileName(sourcePath); 
            if (!fileName.EndsWith(".png") && !fileName.EndsWith(".jpg") && !fileName.EndsWith(".jpeg") && !fileName.EndsWith(".webp") && !fileName.EndsWith(".PNG") && !fileName.EndsWith(".JPG") && !fileName.EndsWith(".JPEG") && !fileName.EndsWith(".WEBP"))
                continue;
            string destPath = Path.Combine(backgroundsPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }
    }

    /// <summary>
    /// Allow users to add a new background image.
    /// </summary>
    public void ImportBackground()
    {
        var Paths = StandaloneFileBrowser.OpenFilePanel("Import Background", "", new[] { new ExtensionFilter("Images", "png", "jpg", "webp") }, true);

        if (Paths.Length == 0) return;
       
        foreach (var sourcePath in Paths)
        {
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(backgroundsPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

        Refresh();

    }

    /// <summary>
    /// Allow users to add a new background image.
    /// </summary>
    public async void ImportBackgroundfromURL(string fileName, string downloadURL)
    {
        Texture2D texture = await DownloadTexture(downloadURL);

        Debug.Log("Saving " + fileName);
        string destPath = Path.Combine(backgroundsPath, fileName + ".png");
        Debug.Log("Downloading from " + downloadURL + " to " + fileName + " | " + destPath);
        File.WriteAllBytes(destPath, texture.EncodeToPNG());
        
        Refresh();
    }

    public async Task<Texture2D> DownloadTexture(string url)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return DownloadHandlerTexture.GetContent(request);
    }

    /// <summary>
    /// Refresh the list of background images.
    /// </summary>
    public void Refresh()
    {
        //Empty the current backgrounds list
        backgrounds.Clear();

        //If there are no files, don't do anything.
        if (!Directory.Exists(backgroundsPath))
            return;

        foreach (var file in Directory.GetFiles(backgroundsPath))
        {
            Debug.Log(file);
            if (!file.EndsWith(".png") && !file.EndsWith(".jpg") && !file.EndsWith(".jpeg") && !file.EndsWith(".webp") && !file.EndsWith(".PNG") && !file.EndsWith(".JPG") && !file.EndsWith(".JPEG") && !file.EndsWith(".WEBP"))
                continue;
            backgrounds.Add(new string(Path.GetFileName(file)));
            Debug.Log(Path.GetFileName(file));
        }

        bList.Refresh();
    }

    /// <summary>
    /// Create and Load a Sprite to use for a background.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>The file name for the background to be loaded</returns>
    public Sprite LoadSprite(string fileName)
    {
        byte[] data = File.ReadAllBytes(Path.Combine(backgroundsPath, fileName));

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data);

        tex.filterMode = FilterMode.Bilinear;

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
    }

    public void Delete(string fileName)
    {
        File.Delete(Path.Combine(backgroundsPath, fileName));
        backgrounds.Remove(fileName);
        bList.Refresh();
    }
}
