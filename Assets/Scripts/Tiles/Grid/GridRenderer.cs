using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridRenderer : MonoBehaviour
{
    public static GridRenderer Instance;
    private GridTile[,] overLayMatrix = new GridTile[10, 10];
    private GridTile[,,] tileMatrix = new GridTile[10, 10, 1];
    private InputAction leftClick;
    public GridTile currentTile = null;
    public GridTile selectedTile = null;
    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private GridTile overlayPrefab;
    [SerializeField] private Transform overlayParent;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform layerPrefab;
    [SerializeField] Camera cam;
    private bool onGrid;

    private Color defaultColor = new Color(0, 0, 0, 0f);
    private Color hoverColor = new Color(0, 0, 0, .5f);

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
                HoverOnTile(overLayMatrix[posInGrid.x, posInGrid.y]);
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
                    currentTile.sRend.color = defaultColor;
                    currentTile = null;
                }
            }

            //Use the tool on the selected tile
            if (leftClick.IsPressed() && currentTile != null)
            {
                GridManager.Instance.UseTool(tileMatrix[currentTile.gridPosition.x, currentTile.gridPosition.y, 0]);
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

            currentTile.sRend.color = defaultColor;
            currentTile = null;
        }

    }

    public void LoadGridMap(GridMap gridMap)
    {
        int gridWidth = gridMap.width;
        int gridHeight = gridMap.height;

        GridSizer.Instance.LoadSize(gridWidth, gridHeight);

        ResetGridTiles(gridMap, gridWidth, gridHeight);
    }

    public void ResetGridTiles(GridMap gridMap, int gridWidth, int gridHeight)
    {
        //Debug.Log("Loaded " + gridMap);

        foreach (Transform child in tileParent)
        {
            Destroy(child.gameObject);
        }

        GridTile[,,] newMatrix = new GridTile[gridWidth, gridHeight, gridMap.tileLayers.Count];

        float xStartPos = 0.5f - gridWidth / 2;
        float yStartPos = 0.5f - gridHeight / 2;
        int iStartOld = tileMatrix.GetLength(0) / 2;
        int jStartOld = tileMatrix.GetLength(1) / 2;

        //Instantiate the grid overlay
        overLayMatrix = new GridTile[gridWidth, gridHeight];
        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                overLayMatrix[i, j] = Instantiate<GridTile>(overlayPrefab, new Vector2(xStartPos + i, yStartPos + j), Quaternion.identity, overlayParent);
                overLayMatrix[i, j].Setup(0, i, j);
            }
        }


        int l = 0;

        foreach (TileLayer layer in gridMap.tileLayers)
        {

            Transform layerParent = Instantiate<Transform>(layerPrefab, tileParent);
            layerParent.name = layer.layerName;

            if (layer.hide)
            {
                layerParent.gameObject.SetActive(false);
            }

            Debug.Log("Setting up layer #" + l + ": " + layer);

            for (int j = 0; j < gridHeight; j++)
            {
                for (int i = 0; i < gridWidth; i++)
                {
                    newMatrix[i, j, l] = Instantiate<GridTile>(tilePrefab, new Vector2(xStartPos + i, yStartPos + j), Quaternion.identity, layerParent);
                    newMatrix[i, j, l].Setup(gridMap.tileLayers[l].tiles[i + j * gridWidth], i, j);
                }
            }

            l++;
        }

        tileMatrix = newMatrix;

    }

    public void HoverOnTile(GridTile newTile)
    {
        //Debug.Log(newTile);
        if(currentTile!= null)
            currentTile.sRend.color = defaultColor;
        currentTile = newTile;
        newTile.sRend.color = hoverColor;

        //Sets the cursor to the appropriate icon
        if (!onGrid) 
        {
            GridManager.Instance.SetCursor();
            onGrid = true;
        }

    }


}
