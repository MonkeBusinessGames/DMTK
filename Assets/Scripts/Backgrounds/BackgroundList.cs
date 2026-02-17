using UnityEngine;

public class BackgroundList : MonoBehaviour
{
    public RectTransform content;
    public BackgroundButton buttonPrefab;
    [SerializeField] private GameObject backgroundSelector;

    public void OpenSelector()
    {
        backgroundSelector.SetActive(true);
        DMManager.onGrid = false;
    }
    public void CloseSelector()
    {
        backgroundSelector.SetActive(false);
        DMManager.onGrid = true;
    }

    public void Refresh()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach(var bg in BackgroundManager.Instance.backgrounds)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(bg, i);
            i++;
            Debug.Log("new list item " + bg);
        }


        //Resize scroll content transform
        content.sizeDelta = new Vector2(0,20 + (444 * Mathf.Ceil((float)i/3)));
    }

}
