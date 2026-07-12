using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] Image dmBackground;
    [SerializeField] Image playerBackground;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] private GameObject musicPlayer;
    [SerializeField] private TMP_Dropdown nowPlaying;
    private Dictionary<string, TMP_Dropdown.OptionData> musicList = new Dictionary<string, TMP_Dropdown.OptionData>();



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
    }

    public void SetBackground(string fileName)
    {
        Sprite sprite = BackgroundManager.Instance.LoadSprite(fileName);
        dmBackground.sprite = playerBackground.sprite = sprite;
    }

    public void StretchToFit(bool stretch)
    {
        dmBackground.preserveAspect = playerBackground.preserveAspect = !stretch;
    }

    public async void SetMusic(string fileName)
    {
        AudioClip clip = await MusicManager.Instance.LoadMusic(fileName);
        if (clip == null)
            return;
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        musicPlayer.gameObject.SetActive(true);
        nowPlaying.value = nowPlaying.options.IndexOf(musicList[fileName]);

    }

    public async void SetMusic(int index)
    {
        string fileName = nowPlaying.options[index].text;
        AudioClip clip = await MusicManager.Instance.LoadMusic(fileName);
        if (clip == null)
            return;
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        musicPlayer.gameObject.SetActive(true);

    }

    public void AddOptionData(string fileName) 
    { 
    
        TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
        newOption.text = fileName;
        musicList[fileName] = newOption;
        nowPlaying.options.Add(newOption);       

    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicPlayer.gameObject.SetActive(false);
    }

    public void PauseMusic()
    {
        if(musicSource.isPlaying)
            musicSource.Pause();
        else
            musicSource.Play();
    }

    public async void PlaySFX(string fileName)
    {
        AudioClip clip = await SFXManager.Instance.Loadsfx(fileName);
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void ExitDMTK()
    {
        Application.Quit();
    }
}
