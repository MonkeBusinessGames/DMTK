using UnityEngine;

public class BackgroundList : MonoBehaviour
{
    public Transform content;
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
            if(child.name != "Create New")
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
    }

}
