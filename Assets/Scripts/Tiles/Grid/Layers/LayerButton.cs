using TMPro;
using UnityEngine;

public class LayerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonName;
    private string layerName;
    [SerializeField] private GameObject hiddenIcon;

    public void Setup(string name, int listPosition, bool layerHidden)
    {
        layerName = name;
        buttonName.text = layerName;
        if (layerHidden)
            hiddenIcon.SetActive(true);
        else
            hiddenIcon.SetActive(false);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 - 120 * listPosition);
    }

    public void Delete()
    {
        LayerSelector.Instance.Delete(layerName);
    }

    public void MoveUp()
    {
        LayerSelector.Instance.MoveLayerUp(layerName);

    }

    public void MoveDown()
    {
        LayerSelector.Instance.MoveLayerDown(layerName);
    }

    public void ToggleHide()
    {
        if (hiddenIcon.activeSelf)
        {

            hiddenIcon.SetActive(false);
            LayerSelector.Instance.ShowLayer(layerName);
        }
        else
        {
            hiddenIcon.SetActive(true);
            LayerSelector.Instance.HideLayer(layerName);
        }

    }
}