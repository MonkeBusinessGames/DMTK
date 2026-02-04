using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PaletteButton : MonoBehaviour
{
    [SerializeField] private Image preview;
    [SerializeField] private TMP_Text buttonName;
    private string palette;

    public void Setup(string paletteData, Sprite mainSprite, int listPosition)
    {
        palette = paletteData;
        preview.sprite = mainSprite;
        buttonName.text = palette;

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
