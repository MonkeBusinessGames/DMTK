using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridManager; 
    private ToolState currentTool;
    [SerializeField] private RectTransform[] toolButtons;
    [SerializeField] RectTransform toolSelectRect;
    [SerializeField] SpriteRenderer defaultGrid;
    private int selectedTile;
    private TileButton selectedButton;
    public static GridManager Instance;
    public List<GridTile> selectedGridTiles;
    private List<GridTile> filledTiles = new List<GridTile>();

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

    public void ToggleGridMap()
    {
        if (GridRenderer.Instance.gameObject.activeSelf)
            GridRenderer.Instance.gameObject.SetActive(false);
        else
            GridRenderer.Instance.gameObject.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTool(int newTool)
    {
        toolSelectRect.position = toolButtons[newTool].position;
        currentTool = (ToolState) newTool;
    }

    public void SetCursor()
    {
        CursorController.Instance.SetCursor(currentTool);
    }

    public void ResetCursor()
    {
        CursorController.Instance.SetCursor(0);
    }

    public void OpenGridTools()
    {
        gridManager.SetActive(true);
        CursorController.Instance.SetCursor(ToolState.Select);
    }

    public void CloseGridTools()
    {
        gridManager.SetActive(false);
        SetTool(0);
    }

    public ToolState UseTool(GridTile gridTile)
    {

        switch (CursorController.Instance.tool)
        {
            case ToolState.Select:
                Select(gridTile);
                break;
            case ToolState.Paint:
                Paint(gridTile);
                break;
            case ToolState.Box:
                BoxFill(gridTile);
                break;
            case ToolState.Fill:
                FloodFill(gridTile);    
                break;
            case ToolState.Erase:
                Erase(gridTile);
                break;
            case ToolState.Drag:
                break;
            case ToolState.Paste:
                Paste(gridTile);
                break;

        }
        return currentTool;
    }

    private void Select(GridTile gridTile)
    {
    }

    public void Paint(GridTile gridTile)
    {
        gridTile.SetTile(selectedTile);
    }

    private void BoxFill(GridTile gridTile)
    {
        GridRenderer.Instance.SetBoxStart();
    }

    private void FloodFill(GridTile gridTile)
    {
        int checkTile = gridTile.tileID;
        if (checkTile == selectedTile)
            return;

            FloodCheck(checkTile, gridTile);
        

        filledTiles = new List<GridTile>();
    }

    private void FloodCheck(int checkID, GridTile gridTile)
    {
        if (filledTiles.Contains(gridTile))
            return;

        filledTiles.Add(gridTile);

        Debug.Log("Starting flood check for against " + checkID + " for tile: " + gridTile);
  
        if(checkID == gridTile.tileID)
        {
            Paint(gridTile);

            Debug.Log(gridTile + " was flood filled with " + selectedTile);

            GridTile check = null;

            if (GridRenderer.Instance.CheckTile(gridTile.gridX - 1, gridTile.gridY, gridTile.gridLayer, out check))
                FloodCheck(checkID, check);

            if (GridRenderer.Instance.CheckTile(gridTile.gridX + 1, gridTile.gridY, gridTile.gridLayer, out check))
                FloodCheck(checkID, check);

            if (GridRenderer.Instance.CheckTile(gridTile.gridX, gridTile.gridY - 1, gridTile.gridLayer, out check))
                FloodCheck(checkID, check);

            if (GridRenderer.Instance.CheckTile(gridTile.gridX, gridTile.gridY + 1, gridTile.gridLayer, out check))
                FloodCheck(checkID, check);

        }

        /*      foreach (GridTile tile in GridRenderer.Instance.GetAdjacentTiles(gridTile.gridX, gridTile.gridY, gridTile.gridLayer))
        {
            if (tile == null)
            {
                Debug.Log("Tile was null");
                continue;
            }

            if (filledTiles.Contains(tile))
            {
                Debug.Log("Tile was already checked");
                continue;
            }

            filledTiles.Add(tile);
                
            if (tile.tileID == checkID)
            {
                gridTile.SetTile(selectedTile);
                FloodCheck(checkID, tile);
                Debug.Log(tile + " was flood filled with " + selectedTile);
            }                
        }*/
    }

    private void Erase(GridTile gridTile)
    {
        gridTile.EraseTile(defaultGrid);

    }

    private void Paste(GridTile gridTile)
    {

    }

    public void SelectTile(TileButton tileButton)
    {
        if(selectedButton != null)
            selectedButton.Unselect();
        selectedTile = tileButton.tile;
        selectedButton = tileButton;
    }

    
}
