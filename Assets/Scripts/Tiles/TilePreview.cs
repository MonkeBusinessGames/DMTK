using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TilePreview : MonoBehaviour
{
    [SerializeField] private Image preview;
    [SerializeField] private TMP_Text buttonName;
    private float tile;

    public void Setup(float tileID, string tileName, Sprite previewSprite, int listPosition)
    {
        tile = tileID;
        preview.sprite = previewSprite;
        buttonName.text = tileName;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 150 * (listPosition % 5), 75 - 150 * Mathf.Floor(listPosition / 5));
    }

    public void Delete()
    {
        PaletteManager.Instance.DeleteTile(tile);
        Destroy(this);
    }

    private void OnDestroy()
    {
        if (preview == null) return;

        //Break the sprite reference
        var sprite = preview.sprite;
        preview.sprite = null;

        //If the sprite exists, destory it
        if (sprite != null)
        {
            var tex = sprite.texture;
            Destroy(sprite);

            //If the texture exists, destroy it
            if (tex != null)
                Destroy(tex);
        }
    }
}
