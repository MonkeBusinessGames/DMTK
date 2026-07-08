using System.Linq;
using TMPro;
using UnityEngine;

public class GridSizer : MonoBehaviour
{
    public static GridSizer Instance;

    public static int gridWidth = 10;
    public static int gridHeight = 10;

    [SerializeField] private SpriteRenderer sRend;

    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject sizerWindow;

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
        widthInput.text = gridWidth.ToString();
        heightInput.text = gridHeight.ToString();
        errorText.SetActive(false);
    }

    public void SetSize()
    {
        try
        {
            int newWidth = int.Parse(widthInput.text);
            int newHeight = int.Parse(heightInput.text);
            if(newWidth > 100 || newWidth < 2 || newHeight > 100 || newHeight < 2 || newWidth % 2 != 0 || newHeight % 2 != 0)
            {
                //If the value is out of range, show the error text and don't do anything else.
                errorText.SetActive(true);
                return;
            }
            gridWidth = newWidth;
            gridHeight = newHeight;
        }
        catch (System.FormatException)
        {
            //If there is an error, show the error text and don't do anything else.
            errorText.SetActive(true);
            return;
        }



        //Hide the error text
        errorText.SetActive(false);

        //Set the size of the grid loaded gridMap
        GridSelector.Instance.loadedGridMap.UpdateSize(gridWidth, gridHeight);

        UnityEngine.Debug.Log("Check1 Tile Layer " + GridSelector.Instance.loadedGridMap.tileLayers[0]);

        //Change the size of the grid itself
        sRend.size = new Vector2(gridWidth, gridHeight);
        GridRenderer.Instance.ResetGridTiles(GridSelector.Instance.loadedGridMap, gridWidth, gridHeight);
        //UnityEngine.Debug.Log("Check2 Tile Layer " + GridSelector.Instance.loadedGridMap.tileLayers[0]);

        CloseWindow();
    }

    public void LoadSize(int newWidth, int newHeight)
    {
        gridWidth = newWidth;
        gridHeight = newHeight;

        widthInput.text = gridWidth.ToString();
        heightInput.text = gridHeight.ToString();

        sRend.size = new Vector2(gridWidth, gridHeight);
    }

    public void OpenWindow()
    {
        sizerWindow.SetActive(true);
    }
    public void CloseWindow()
    {
        sizerWindow.SetActive(false);
    }
}
