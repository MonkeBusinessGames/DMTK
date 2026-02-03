using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TilePreview : MonoBehaviour
{
    [SerializeField] private Image preview;
    [SerializeField] private TMP_Text buttonName;
    private string fileName;

    public void Setup(string file, Sprite previewSprite, int listPosition)
    {
        fileName = file;
        preview.sprite = previewSprite;
        buttonName.text = fileName;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 200 * (listPosition % 4), 50 - 100 * Mathf.Floor(listPosition / 4));
    }

    public void Delete()
    {
        PaletteManager.Instance.DeleteTile(fileName);
        Destroy(this);
    }
}
