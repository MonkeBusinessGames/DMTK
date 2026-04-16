using TMPro;
using UnityEngine;

public class LayerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonName;
    private string layerName;

    public void Setup(string name, int listPosition)
    {
        layerName = name;
        buttonName.text = layerName;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 - 120 * listPosition);
    }

    public void OnClick()
    {
        LayerSelector.Instance.SelectLayer(layerName);
    }

    public void Delete()
    {
        LayerSelector.Instance.Delete(layerName);
    }

    public void MoveUp()
    {

    }

    public void MoveDown()
    {

    }
}
