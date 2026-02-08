using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridManager;
    private float selectedTile;
    private TileButton selectedButton;
    public static GridManager Instance;
    [SerializeField] private TileButton tilePrefab;
    [SerializeField] private Transform tilePanel;

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
        CursorController.Instance.SetCursor((ToolState) newTool);
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

    private void Highlight()
    {

    }

    private void Paint()
    {

    }

    private void BoxFill()
    {

    }

    private void FloodFill()
    {

    }

    private void Erase()
    {

    }

    public void SelectTile(TileButton tileButton)
    {
        if(selectedButton != null)
            selectedButton.Unselect();
        selectedTile = tileButton.tile;
        selectedButton = tileButton;
    }

    public void ClearTiles()
    {

        foreach (Transform child in tilePanel)
            Destroy(child.gameObject);
    }

    public void LoadTile(float tile, int placement)
    {
        var btn = Instantiate(tilePrefab, tilePanel);
        btn.Setup(tile, placement);
    }
}
