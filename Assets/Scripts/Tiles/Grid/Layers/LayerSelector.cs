using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class LayerSelector : MonoBehaviour
{

    public static LayerSelector Instance;
    [SerializeField] private GameObject selector;
    public List<string> layerList = new();
    public RectTransform content;
    public LayerButton buttonPrefab;

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

        //Refresh the layers selector list
        RefreshSelector();

    }

    public void Delete(string fileName)
    {
        //Remove layer from loaded gridmap


        layerList.Remove(fileName);
        RefreshSelector();
    }

    public void OpenSelector()
    {
        selector.SetActive(true);
    }
    public void CloseSelector()
    {
        selector.SetActive(false);
    }

    public void RefreshSelector()
    {

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (var layer in layerList)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.Setup(layer, i);
            i++;
            Debug.Log("new list item " + layer);
        }

        //Resize scroll content transform
        content.sizeDelta = new Vector2(0, 20 + (120 * i));
    }

    public void SelectLayer(string layerName)
    {

    }

    public void AddLayer()
    {

    }

    public void MoveLayerUp(string layerName)
    {

    }

    public void MoveLayerDown()
    {

    }
}
