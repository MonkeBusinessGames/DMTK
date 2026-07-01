using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public SpriteRenderer sRend;
    public TileData tileData;
    public int tileID = 0;

    public void IsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public override string ToString()
    {
        return gridPosition + sRend.name;
    }

    public void Setup(int tile, int positionX,  int positionY)
    {
        UpdateSprite(tile);
        gridPosition = new Vector2Int(positionX, positionY);
        
        Debug.Log("Tile " + tile + " created at " +  gridPosition);
    }

    public void SetTile(int tile)
    {
        UpdateSprite(tile);

        //Update the tile in the GridMap data
        GridSelector.Instance.loadedGridMap.SetTile(gridPosition.x, gridPosition.y, 0, tile);
    }

    private void UpdateSprite(int tile)
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

        //Set the tile data
        tileID = tile;
        tileData = PaletteManager.Instance.loadedTileData[tile];
    }

    public void EraseTile(SpriteRenderer whiteSprite)
    {

        //Clear the sprite
        sRend.sprite = whiteSprite.sprite;

        //Set the tile data
        tileID = 0;
        tileData = null;

        //Update the tile in the GridMap data
        GridSelector.Instance.loadedGridMap.tileLayers[0].tiles[gridPosition.x + gridPosition.y * GridSizer.gridWidth] = new int();
    }
}
