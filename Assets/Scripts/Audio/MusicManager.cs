using UnityEngine;
using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private string musicPath;
    public List<string> musicList = new();
    public Transform content;
    public MusicButton buttonPrefab;
    Dictionary<string, AudioClip> musicCache = new();

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

        //Define the music path
        musicPath = Path.Combine(Application.persistentDataPath, "Music");

        //If the folder for storing musics doesn't exist, create it.
        if (!Directory.Exists(musicPath))
            Directory.CreateDirectory(musicPath);

        //Refresh the musics list
        Refresh();

    }

    /// <summary>
    /// Allow users to add a new music image.
    /// </summary>
    public void ImportMusic()
    {
        var Paths = StandaloneFileBrowser.OpenFilePanel("Import Music", "", new[] { new ExtensionFilter("Audio", "mp3", "wav", "ogg") }, true);

        if (Paths.Length == 0) return;

        foreach (var sourcePath in Paths)
        {
            string fileName = Path.GetFileName(sourcePath);
            string destPath = Path.Combine(musicPath, fileName);
            Debug.Log(sourcePath + " | " + fileName + " | " + destPath);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

        Refresh();

    }

    /// <summary>
    /// Refresh the list of music files.
    /// </summary>
    public void Refresh()
    {
        //Empty the current music list
        musicList.Clear();

        //If there are no files, don't do anything.
        if (!Directory.Exists(musicPath))
            return;

        foreach (var file in Directory.GetFiles(musicPath))
        {
            Debug.Log(file);
            if (!file.EndsWith(".png") && !file.EndsWith(".mp3") && !file.EndsWith(".MP#") && !file.EndsWith(".wav") && !file.EndsWith(".WAV") && !file.EndsWith(".ogg") && !file.EndsWith(".OGG"))
                continue;
            musicList.Add(new string(Path.GetFileName(file)));
            Debug.Log(Path.GetFileName(file));
        }

        RefreshSelector();
    }

    public async Task<AudioClip> LoadMusic(string fileName)
    {
        if (musicCache.TryGetValue(fileName, out var cached))
            return cached;

        string url = "file://" + Path.Combine(musicPath, fileName);

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
        musicCache[fileName] = clip;
        return clip;
    }

    public void Delete(string fileName)
    {
        File.Delete(Path.Combine(musicPath, fileName));
        musicList.Remove(fileName);
        musicCache.Remove(fileName);
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
        foreach (var bg in musicList)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(bg, i);
            i++;
            Debug.Log("new list item " + bg);
        }
    }
}
