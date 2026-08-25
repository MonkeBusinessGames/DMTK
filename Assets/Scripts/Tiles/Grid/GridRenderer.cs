using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Assets.Scripts;
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
    [SerializeField] private SceneInput sceneData;
    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private GridTile overlayPrefab;
    [SerializeField] private Transform overlayParent;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform layerPrefab;
    [SerializeField] Camera cam;
    private bool onGrid;
    private int topLayerIndex = 0;
    private Color defaultColor = new Color(0, 0, 0, 0f);
    private Color hoverColor = new Color(0, 0, 0, .3f);
    private Color highlightColor = new Color(0, 0, 0, .5f);
    public ToolState selectState = ToolState.None;
    private GridTile startGridTile = null;
    private GridTile endGridTile = null;
    private List<GridTile> highlightedTiles = new List<GridTile>();
    private GridTile[] adjacentTiles = new GridTile[4];

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

        if (sceneData.isHoveringViewport)
        {
            //Debug.Log("Mouse is not over UI");
            
            //Get the position in the grid based on the point position
            Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //Vector2Int posInGrid = new Vector2Int(Mathf.FloorToInt(mousePosition.x) + tileMatrix.GetLength(0) / 2, Mathf.FloorToInt(mousePosition.y) + tileMatrix.GetLength(1) / 2);

            Vector2Int posInGrid = sceneData.gridPosition;
            Debug.Log(posInGrid.ToString());

            //Debug.Log("Mouse" + mousePosition + "| Grid " + posInGrid);
            try
            {
                //Highlight all the tiles being selected
                if (selectState == ToolState.Select)
                    HighlightTile(overLayMatrix[posInGrid.x, posInGrid.y]);

                //Highlight all the tiles between gridTiles
                else if(selectState == ToolState.Box)
                    HighlightBox(overLayMatrix[posInGrid.x, posInGrid.y]);

                else
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
            if (leftClick.WasPressedThisFrame() && currentTile != null)
            {
                ClearHighlight();
                selectState = GridManager.Instance.UseTool(tileMatrix[currentTile.gridX, currentTile.gridY, topLayerIndex]);
            }

            else if (leftClick.WasReleasedThisFrame())
            {
                switch (selectState)
                {
                    case ToolState.Select:
                        break;
                    case ToolState.Paint:
                        break;
                    case ToolState.Box:
                        //Paint all the highlighted tiles in the box
                        foreach (GridTile tile in highlightedTiles)
                        {
                            tile.sRend.color = defaultColor;
                            GridManager.Instance.Paint(tileMatrix[tile.gridX, tile.gridY, topLayerIndex]);
                        }

                        highlightedTiles = new List<GridTile>();
                        break;
                    case ToolState.Fill:
                        break;
                    case ToolState.Erase:
                        break;
                    case ToolState.Drag:
                        break;
                    case ToolState.Paste:
                        break;
                }

                selectState = ToolState.None;
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
        sceneData.xOffset = gridWidth / 2;
        sceneData.yOffset = gridHeight / 2;

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
                overLayMatrix[i, j].Setup(0, i, j, 0);
            }
        }


        int l = 0;
        topLayerIndex = 0;
        foreach (TileLayer layer in gridMap.tileLayers)
        {

            Transform layerParent = Instantiate<Transform>(layerPrefab, tileParent);
            layerParent.name = layer.layerName;

            if (layer.hide)
            {
                layerParent.gameObject.SetActive(false);
                if(topLayerIndex == l)
                    topLayerIndex++;
            }

            Debug.Log("Setting up layer #" + l + ": " + layer);

            for (int j = 0; j < gridHeight; j++)
            {
                for (int i = 0; i < gridWidth; i++)
                {
                    newMatrix[i, j, l] = Instantiate<GridTile>(tilePrefab, new Vector2(xStartPos + i, yStartPos + j), Quaternion.identity, layerParent);
                    newMatrix[i, j, l].Setup(gridMap.tileLayers[l].tiles[i + j * gridWidth], i, j, l);
                    Debug.Log("GridTile set up at " + i + ", " + j + ", " + l + ": " + newMatrix[i, j, l]);
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
        {
            if (highlightedTiles.Contains(currentTile))
                currentTile.sRend.color = highlightColor;

            else
                currentTile.sRend.color = defaultColor;

        }
        currentTile = newTile;
        newTile.sRend.color = hoverColor;

        //Sets the cursor to the appropriate icon
        if (!onGrid) 
        {
            GridManager.Instance.SetCursor();
            onGrid = true;
        }

    }

    public void HighlightTile(GridTile newTile)
    {
        currentTile = newTile;
        highlightedTiles.Add(newTile);
        newTile.sRend.color = highlightColor;
    }

    public void SetBoxStart()
    {
        startGridTile = overLayMatrix[currentTile.gridX, currentTile.gridY];
        startGridTile.sRend.color = highlightColor;
        Debug.Log("Box Start set to " + startGridTile);
    }

    public void HighlightBox(GridTile newTile)
    {
        if (newTile != endGridTile)
        {
            List<GridTile> tempTiles = new List<GridTile>();

            endGridTile = newTile;
            int minX = Mathf.Min(endGridTile.gridX, startGridTile.gridX);
            int maxX = Mathf.Max(endGridTile.gridX, startGridTile.gridX);
            int minY = Mathf.Min(endGridTile.gridY, startGridTile.gridY);
            int maxY = Mathf.Max(endGridTile.gridY, startGridTile.gridY);

            //create a list of all tiles in box
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    overLayMatrix[x, y].sRend.color = highlightColor;
                    tempTiles.Add(overLayMatrix[x, y]);
                }
            }

            //reset colors for previous highlighted tiles no longer in box
            foreach (GridTile tile in highlightedTiles)
            {
                if (!tempTiles.Contains(tile))
                    tile.sRend.color = defaultColor;
            }

            //set highlighted tiles = new box
            highlightedTiles = tempTiles;
        }
    }

    public void ClearHighlight()
    {
        //reset colors for previous highlighted tiles no longer in box
        foreach (GridTile tile in highlightedTiles)
        {  
            tile.sRend.color = defaultColor;
        }
    }

    public GridTile[] GetAdjacentTiles(int x, int y, int z)
    {
        Debug.Log("Checking adjacent tiles for " + x + "," + y + "," + z);

        try
        {
            adjacentTiles[0] = tileMatrix[x - 1, y , z];
            Debug.Log("Adjacent tile found for " + (x - 1)  + "," + y+ "," + z + " ; " + adjacentTiles[0]);
        }
        catch (IndexOutOfRangeException)
        {
            adjacentTiles[0] = null;
        }
        try
        {
            adjacentTiles[1] = tileMatrix[x + 1, y, z];
            Debug.Log("Adjacent tile found for " + (x + 1) + "," + y + "," + z + " ; " + adjacentTiles[0]);
        }
        catch (IndexOutOfRangeException)
        {
            adjacentTiles[1] = null;
        }
        try
        {
            adjacentTiles[2] = tileMatrix[x, y - 1, z];
            Debug.Log("Adjacent tile found for " + x+ "," + (y - 1) + "," + z + " ; " + adjacentTiles[0]);
        }
        catch (IndexOutOfRangeException)
        {
            adjacentTiles[2] = null;
        }
        try
        {
            adjacentTiles[3] = tileMatrix[x, y + 1, z];
            Debug.Log("Adjacent tile found for " + x + "," + (y + 1) + "," + z + " ; " + adjacentTiles[0]);
        }
        catch (IndexOutOfRangeException)
        {
            adjacentTiles[3] = null;
        }
        return adjacentTiles;
    }

    public bool CheckTile(int x, int y, int z, out GridTile tile)
    {
        tile = null;
        try
        {
            tile = tileMatrix[x, y, z];
            Debug.Log("Adjacent tile found for " + x + "," + y + "," + z + " ; " + tile);
            return true;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

}
