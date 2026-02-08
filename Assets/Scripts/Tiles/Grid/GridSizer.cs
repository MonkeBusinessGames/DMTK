using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GridSizer : MonoBehaviour
{
    public static GridSizer Instance;

    public Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private SpriteRenderer sRend;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private TMP_Text widthValue;
    [SerializeField] private TMP_Text heightValue;
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
        widthSlider.value = gridSize.x / 2;
        heightSlider.value = gridSize.y / 2;

        widthValue.text = gridSize.x.ToString();
        heightValue.text = gridSize.y.ToString();
    }
    public void ChangeHeight()
    {
        gridSize.y = (int) heightSlider.value * 2;
        sRend.size = gridSize;
        heightValue.text = gridSize.y.ToString();
        GridSelector.Instance.loadedGridMap.height = gridSize.y;
    }

    public void ChangeWidth()
    {
        gridSize.x = (int) widthSlider.value * 2;
        sRend.size = gridSize;
        widthValue.text = gridSize.x.ToString();
        GridSelector.Instance.loadedGridMap.width = gridSize.x;
    }

    public void LoadSize(int gridWidth, int gridHeight) 
    {
        gridSize = new Vector2Int(gridWidth, gridHeight);

        widthSlider.value = gridSize.x / 2;
        heightSlider.value = gridSize.y / 2;

        widthValue.text = gridSize.x.ToString();
        heightValue.text = gridSize.y.ToString();

        sRend.size = gridSize;
    }

}
