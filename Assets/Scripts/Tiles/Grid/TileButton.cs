using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    [SerializeField] private Image preview;
    public int tile;

    public void Setup(int tileID, int listPosition)
    {
        tile = tileID;
        preview.sprite = PaletteManager.Instance.loadedTileSprites[tile];

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-100 + 200 * (listPosition % 2), -20 - 200 * Mathf.Floor(listPosition / 2));
    }

    public void OnClick()
    {
        GridManager.Instance.SelectTile(this);
        preview.color = Color.grey;
    }

    public void Unselect()
    {
        preview.color = Color.white;
    }

}
