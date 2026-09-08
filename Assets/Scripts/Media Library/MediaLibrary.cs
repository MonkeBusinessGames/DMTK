using TMPro;
using UnityEngine;


public class MediaLibrary : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mediaSelector;
    [SerializeField] private GameObject audioContent;
    [SerializeField] private GameObject imageContent;
    [SerializeField] private BackgroundManager backgroundLibrary;
    [SerializeField] private MusicManager musicLibrary;
    [SerializeField] private SFXManager sfxLibrary;
    [SerializeField] private ImageSearchManager imageSearchManager;
    [SerializeField] private AudioSearchManager audioSearchManager;
    [SerializeField] private GameObject stretchToFit;
    private int mediaIndex;

    private void Start()
    {
        SelectMedia(0);
        mediaSelector.SetValueWithoutNotify(0);
    }

    public void SelectMedia(int media)
    {
        mediaIndex = media;
        switch (mediaIndex)
        {
            case 0: // background
                backgroundLibrary.RefreshSelector();
                stretchToFit.SetActive(true);
                audioContent.SetActive(false);
                imageContent.SetActive(true);
                break;
            case 1: // music
                musicLibrary.RefreshSelector();
                stretchToFit.SetActive(false);
                audioContent.SetActive(true);
                imageContent.SetActive(false);
                break;
            case 2: // sfx
                sfxLibrary.RefreshSelector();
                stretchToFit.SetActive(false);
                audioContent.SetActive(true);
                imageContent.SetActive(false);
                break;
        }
    }

    public void Upload()
    {
        switch (mediaIndex)
        {
            case 0: // background
                backgroundLibrary.ImportBackground();
                break;
            case 1: // music
                musicLibrary.ImportMusic();
                break;
            case 2: // sfx
                sfxLibrary.Importsfx();
                break;
        }
    }
    public void Search()
    {
        switch (mediaIndex)
        {
            case 0: // background
                imageSearchManager.OpenSearchPage();
                break;
            case 1: // music
                audioSearchManager.OpenSearchPage(false);
                break;
            case 2: // sfx
                audioSearchManager.OpenSearchPage(true);
                break;
        }
    }
}
