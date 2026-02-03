using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GridSizer : MonoBehaviour
{
    public static Vector2 gridSize = new Vector2Int(10, 10);
    [SerializeField] private SpriteRenderer sRend;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private TMP_Text widthValue;
    [SerializeField] private TMP_Text heightValue;

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
    }

    public void ChangeWidth()
    {
        gridSize.x = (int) widthSlider.value * 2;
        sRend.size = gridSize;
        widthValue.text = gridSize.x.ToString();
    }

    public void LoadSize(Vector2Int newSize) 
    {
        gridSize = newSize;

        widthSlider.value = gridSize.x / 2;
        heightSlider.value = gridSize.y / 2;

        widthValue.text = gridSize.x.ToString();
        heightValue.text = gridSize.y.ToString();

        sRend.size = gridSize;
    }

}
