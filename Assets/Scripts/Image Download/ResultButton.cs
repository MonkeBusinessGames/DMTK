using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Assets.Scripts.Image_Download
{
	public class ResultButton: MonoBehaviour
	{

        [SerializeField] private Image preview;
        [SerializeField] private TMP_Text buttonName;
        private string title;
        private string downloadURL;

        public void Setup(string name, Sprite previewSprite, string imageURL)
        {
            title = name;
            preview.sprite = previewSprite;
            buttonName.text = name;
            downloadURL = imageURL;

        }

        public void Download()
        {
            ImageSearchManager.Instance.SelectSearchResult(title, downloadURL);
        }

        private void OnDestroy()
        {
            Destroy(preview.sprite.texture);
            Destroy(preview.sprite);
        }
    }
}