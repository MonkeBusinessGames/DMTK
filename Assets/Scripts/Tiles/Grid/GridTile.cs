using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public SpriteRenderer sRend;
    public TileData tileData;
    public float tileID;

    public void IsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public override string ToString()
    {
        return gridPosition + sRend.name;
    }

    public void SetTile(float tile)
    {
        if (tileID == tile)
            return;
        sRend.sprite = PaletteManager.Instance.loadedTileSprites[tile];

        //Get the size of the sprite
        Vector2 spriteWorldSize = sRend.sprite.bounds.size;

        ////Get the minimum scale to stretch
        //float scale = Mathf.Min(1f / spriteWorldSize.x, 1f / spriteWorldSize.y);

        ////Scle the transform to make sure the tile fits within 1 x 1 tile without changing aspect ratio.
        //transform.localScale = new Vector3(scale, scale, 1f);

        // Scale to fit 1x1 tile
        float scaleX = 1f / spriteWorldSize.x;
        float scaleY = 1f / spriteWorldSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        tileData = PaletteManager.Instance.loadedTileData[tile];
    }
}
