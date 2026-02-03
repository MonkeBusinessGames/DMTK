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

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 200 * (listPosition % 4), 50 - 100 * Mathf.Floor(listPosition / 4));
    }

    public void OnClick()
    {
        FindFirstObjectByType<SceneManager>().SetBackground(fileName);
    }

    public void Delete()
    {
        BackgroundManager.Instance.Delete(buttonName.text);
    }


}
