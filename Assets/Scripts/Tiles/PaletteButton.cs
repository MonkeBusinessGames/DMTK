using UnityEngine;
using UnityEngine.UI;

public class PaletteButton : MonoBehaviour
{
    [SerializeField] private Image preview;
    public string palette;

    public void Setup(string paletteData, int listPosition)
    {
        palette = paletteData;
        //preview.sprite = PaletteManager.Instance.loadedTiles[paletteData.mainSprite];

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 200 * (listPosition % 4), 50 - 100 * Mathf.Floor(listPosition / 4));
    }

    public void OnClick()
    {
        PaletteManager.Instance.SelectPalette(palette);
    }

    public void EditPalette()
    {
        PaletteManager.Instance.EditPalette(palette);
    }

}
