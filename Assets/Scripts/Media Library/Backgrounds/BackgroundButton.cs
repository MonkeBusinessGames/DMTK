using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackgroundButton : MonoBehaviour
{
    [SerializeField] private Image preview;
    [SerializeField] private TMP_Text buttonName;
    private string fileName;

    public void Setup(string file, int listPosition)
    {
        fileName = file;
        preview.sprite = BackgroundManager.Instance.LoadSprite(file);
        buttonName.text = fileName;
    }

    public void OnClick()
    {
        FindAnyObjectByType<SceneManager>().SetBackground(fileName);
    }

    public void Delete()
    {
        BackgroundManager.Instance.Delete(buttonName.text);
    }


}
