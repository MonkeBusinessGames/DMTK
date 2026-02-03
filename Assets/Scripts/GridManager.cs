using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject ToolPanel;
    private TileData selectedTile;
    private TileButton selectedButton;
    public static GridManager Instance;
    private void Awake()
    {
        //Prevent duplicates of this object from existing
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Make this object accessible to other objects and don't destory it.
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void ToggleTools()
    {
        if (ToolPanel.activeSelf)
        {
            ToolPanel.SetActive(false);
            CursorController.Instance.SetCursor(ToolState.Select);
        }
        else
            ToolPanel.SetActive(true);

    }

    public void CloseTools()
    {
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
        tileButton.Unselect();
        selectedTile = tileButton.tile;
        selectedButton = tileButton;
    }
}
