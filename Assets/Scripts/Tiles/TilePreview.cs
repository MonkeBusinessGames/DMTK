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
