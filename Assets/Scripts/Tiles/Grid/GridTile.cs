using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public SpriteRenderer sRend;
    public Sprite[] storedSprites;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GridRenderer.Instance.currentTile = this;
    }
    public void IsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
