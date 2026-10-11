using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SFXButton : MonoBehaviour
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
        FindAnyObjectByType<SceneManager>().PlaySFX(fileName);
    }

    public void Delete()
    {
        SFXManager.Instance.Delete(buttonName.text);
    }


}
