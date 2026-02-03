using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    [SerializeField] private Image preview;
    public TileData tile;

    public void Setup(TileData tileData, int listPosition)
    {
        tile = tileData;
        preview.sprite = PaletteManager.Instance.loadedTiles[tile.tileName];

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 200 * (listPosition % 4), 50 - 100 * Mathf.Floor(listPosition / 4));
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
