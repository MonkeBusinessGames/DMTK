using UnityEngine;
using TMPro;
using System.Linq;

public class GridSizer : MonoBehaviour
{
    public static GridSizer Instance;

    public Vector2Int gridSize = new Vector2Int(10, 10);
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
        widthInput.text = gridSize.x.ToString();
        heightInput.text = gridSize.y.ToString();
        errorText.SetActive(false);
    }

    public void SetSize()
    {
        try
        {
            Vector2Int newSize = new Vector2Int(int.Parse(widthInput.text), int.Parse(heightInput.text));
            if(newSize.x > 100 || newSize.x < 2 || newSize.y > 100 || newSize.y < 2 || newSize.x % 2 != 0 || newSize.y % 2 != 0)
            {
                //If the value is out of range, show the error text and don't do anything else.
                errorText.SetActive(true);
                return;
            }
            gridSize = newSize;
        }
        catch (System.FormatException)
        {
            //If there is an error, show the error text and don't do anything else.
            errorText.SetActive(true);
            return;
        }



        //Hide the error text
        errorText.SetActive(false);
        
        //Set the size of the grid renderer object
        GridSelector.Instance.loadedGridMap.width = gridSize.x;
        GridSelector.Instance.loadedGridMap.height = gridSize.y;

        //Change the size of the grid itself
        sRend.size = gridSize;

        CloseWindow();
    }

    public void LoadSize(int gridWidth, int gridHeight)
    {
        gridSize = new Vector2Int(gridWidth, gridHeight);

        widthInput.text = gridSize.x.ToString();
        heightInput.text = gridSize.y.ToString();

        sRend.size = gridSize;
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
