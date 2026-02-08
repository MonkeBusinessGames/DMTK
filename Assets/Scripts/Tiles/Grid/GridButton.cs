using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridButton : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonName;
    private string gridName;

    public void Setup(string name, int listPosition)
    {
        gridName = name;
        buttonName.text = gridName;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40 - 40 * listPosition);
    }

    public void OnClick()
    {
        GridSelector.Instance.SelectGridMap(buttonName.text);
    }

    public void Delete()
    {
        GridSelector.Instance.Delete(buttonName.text);
    }


}
