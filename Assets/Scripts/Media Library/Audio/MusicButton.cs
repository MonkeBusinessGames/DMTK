using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonName;
    private string fileName;

    public void Setup(string file, int listPosition)
    {
        fileName = file;
        buttonName.text = fileName;

    }

    public void OnClick()
    {
        FindAnyObjectByType<SceneManager>().SetMusic(fileName);
    }

    public void Delete()
    {
        MusicManager.Instance.Delete(buttonName.text);
    }


}
