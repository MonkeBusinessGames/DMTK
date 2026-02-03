using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public void SetBackground(string fileName)
    {
        Sprite sprite = BackgroundManager.Instance.LoadSprite(fileName);
        backgroundImage.sprite = sprite;
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
