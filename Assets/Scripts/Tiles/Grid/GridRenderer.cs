using UnityEngine;
using System.Collections.Generic;
public class GridRenderer : MonoBehaviour
{
    public static GridRenderer Instance;
    private Dictionary<Vector2Int, SpriteRenderer> gridSpaces = new();
    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects.
        Instance = this;
    }

    public void LoadGridMap()
    {

    }

    public void SetTile(Vector2Int position, Sprite sprite)
    {
        //Check if a gameobject already exists in this gridspace
        if(!gridSpaces.TryGetValue(position, out var sr))
        {
            //If a gameobject doesn't exist, create one
            GameObject go = new GameObject($"Tile_{position}");
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(position.x, position.y, 0);
            sr = go.AddComponent<SpriteRenderer>();
            gridSpaces[position] = sr;
        }

        //Set gameobject's sprite to the new sprite.
        sr.sprite = sprite;
    }
}
