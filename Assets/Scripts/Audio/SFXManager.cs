using UnityEngine;
using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    private string sfxPath;
    public List<string> sfxList = new();
    public Transform content;
    public SFXButton buttonPrefab;
    Dictionary<string, AudioClip> sfxCache = new();

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

        //Define the sfx path
        sfxPath = Path.Combine(Application.persistentDataPath, "SFX");

        //If the folder for storing sfxs doesn't exist, create it.
        if (!Directory.Exists(sfxPath))
            Directory.CreateDirectory(sfxPath);

        //Refresh the sfxs list
        Refresh();

    }

    /// <summary>
    /// Allow users to add a new sfx image.
    /// </summary>
    public void Importsfx()
    {
        var Paths = StandaloneFileBrowser.OpenFilePanel("Import SFX", "", new[] { new ExtensionFilter("Audio", "mp3", "wav", "ogg") }, true);

        if (Paths.Length == 0) return;
        
        foreach(var sourcePath in Paths) 
        {
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(sfxPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

        Refresh();

    }

    /// <summary>
    /// Refresh the list of sfx files.
    /// </summary>
    public void Refresh()
    {
        //Empty the current sfx list
        sfxList.Clear();

        //If there are no files, don't do anything.
        if (!Directory.Exists(sfxPath))
            return;

        foreach (var file in Directory.GetFiles(sfxPath))
        {
            Debug.Log(file);
            if (!file.EndsWith(".png") && !file.EndsWith(".mp3") && !file.EndsWith(".MP3") && !file.EndsWith(".wav") && !file.EndsWith(".WAV") && !file.EndsWith(".ogg") && !file.EndsWith(".OGG"))
                continue;
            sfxList.Add(new string(Path.GetFileName(file)));
            Debug.Log(Path.GetFileName(file));
        }

        RefreshSelector();
    }

    public async Task<AudioClip> Loadsfx(string fileName)
    {
        if (sfxCache.TryGetValue(fileName, out var cached))
            return cached;

        string url = "file://" + Path.Combine(sfxPath, fileName);

        using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);

        var op = req.SendWebRequest();
        while (!op.isDone)
            await Task.Yield();

        if(req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.error);
            return null;
        }

        AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
        sfxCache[fileName] = clip;
        return clip;
    }

    public void Delete(string fileName)
    {
        File.Delete(Path.Combine(sfxPath, fileName));
        sfxList.Remove(fileName);
        sfxCache.Remove(fileName);
        RefreshSelector();
    }

    public void OpenSelector()
    {
        gameObject.SetActive(true);
    }
    public void CloseSelector()
    {
        gameObject.SetActive(false);
    }

    public void RefreshSelector()
    {
        foreach (Transform child in content)
        {
            if (child.name != "Create New")
                Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var bg in sfxList)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(bg, i);
            i++;
            Debug.Log("new list item " + bg);
        }
    }
}
