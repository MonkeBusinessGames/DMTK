using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridManager;
    private ToolState currentTool;
    [SerializeField] private RectTransform[] toolButtons;
    [SerializeField] RectTransform toolSelectRect;
    private int selectedTile;
    private TileButton selectedButton;
    public static GridManager Instance;
    public GridTile selectedGridTile;

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
        CursorController.Instance.SetCursor(ToolState.Select);
    }

    public void UseTool(GridTile gridTile)
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
    }

    private void Select(GridTile gridTile)
    {
        selectedGridTile = gridTile;
    }

    private void Highlight(GridTile gridTile)
    {

    }

    private void Paint(GridTile gridTile)
    {
        gridTile.SetTile(selectedTile);
    }

    private void BoxFill(GridTile gridTile)
    {

    }

    private void FloodFill(GridTile gridTile)
    {

    }

    private void Erase(GridTile gridTile)
    {

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
