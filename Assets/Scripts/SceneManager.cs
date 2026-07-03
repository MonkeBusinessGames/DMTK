using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] Image dmBackground;
    [SerializeField] Image playerBackground;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;


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

    public async void SetMusic(string fileName)
    {
        AudioClip clip = await MusicManager.Instance.LoadMusic(fileName);
        if (clip == null)
            return;
        musicSource.Stop();
        musicSource.clip = clip;
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
