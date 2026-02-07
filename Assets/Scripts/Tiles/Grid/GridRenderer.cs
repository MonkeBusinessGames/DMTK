using UnityEngine;
using System.Collections.Generic;
public class GridRenderer : MonoBehaviour
{

    private Dictionary<Vector2Int, SpriteRenderer> gridSpaces = new();
    
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
