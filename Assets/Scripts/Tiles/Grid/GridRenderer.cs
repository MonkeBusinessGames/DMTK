using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridRenderer : MonoBehaviour
{
    public static GridRenderer Instance;
    private Dictionary<Vector2Int, SpriteRenderer> gridSpaces = new();
    private GridTile[,] tileMatrix = new GridTile[10, 10];
    private InputAction leftClick;
    public GridTile currentTile = null;
    public GridTile selectedTile = null;
    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] Camera cam;
    private bool onGrid;

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

    private void Start()
    {
        onGrid = false;
        leftClick = InputSystem.actions.FindAction("LeftClick");
    }

    private void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Mouse is not over UI");
            
            //Get the position in the grid based on the point position
            Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2Int posInGrid = new Vector2Int(Mathf.FloorToInt(mousePosition.x) + tileMatrix.GetLength(0) / 2, Mathf.FloorToInt(mousePosition.y) + tileMatrix.GetLength(1) / 2);

            //Debug.Log("Mouse" + mousePosition + "| Grid " + posInGrid);
            try
            {
                //Try showing the hover on the tile
                HoverOnTile(tileMatrix[posInGrid.x, posInGrid.y]);
            }
            catch (IndexOutOfRangeException)
            {

                //Resets the cursor to select
                if (onGrid)
                {
                    GridManager.Instance.ResetCursor();
                    onGrid = false;
                }
                //If the grid position is not valid, reset the current tile
                if (currentTile != null)
                {
                    currentTile.sRend.color = Color.white;
                    currentTile = null;
                }
            }

            //Use the tool on the selected tile
            if (leftClick.IsPressed())
            {
                GridManager.Instance.UseTool(currentTile);
            }
        }

        //Reset the current tile to null
        else if (currentTile != null)
        {
            //Resets the cursor to select
            if (onGrid)
            {
                GridManager.Instance.ResetCursor();
                onGrid = false;
            }

            currentTile.sRend.color = Color.white;
            currentTile = null;
        }

    }

    public void LoadGridMap(GridMap gridMap)
    {
        GridSizer.Instance.LoadSize(gridMap.width, gridMap.height);
        UpdateGridSize(gridMap.width, gridMap.height);
        //Debug.Log("Loaded " + gridMap);
    }

    public void UpdateGridSize(int gridWidth, int gridHeight)
    {

        foreach (Transform child in tileParent)
        {
            Destroy(child.gameObject);
        }

        GridTile[,] newMatrix = new GridTile[gridWidth, gridHeight];
        
        float xStartPos = 0.5f - gridWidth / 2;
        float yStartPos = 0.5f - gridHeight / 2;
        int iStartOld = tileMatrix.GetLength(0) / 2;
        int jStartOld = tileMatrix.GetLength(1) / 2;

        for(int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++) 
            {
                newMatrix[i, j] = Instantiate<GridTile>(tilePrefab, new Vector2(xStartPos + i, yStartPos + j), Quaternion.identity, tileParent);
            }
        }

        tileMatrix = newMatrix;

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

    public void HoverOnTile(GridTile newTile)
    {
        //Debug.Log(newTile);

        if(currentTile!= null)
            currentTile.sRend.color = Color.white;
        currentTile = newTile;
        newTile.sRend.color = Color.gray;

        //Sets the cursor to the appropriate icon
        if (!onGrid) 
        {
            GridManager.Instance.SetCursor();
            onGrid = true;
        }

    }


}
