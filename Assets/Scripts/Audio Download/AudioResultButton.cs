using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using Unity.VisualScripting;
using System;


namespace Assets.Scripts.Image_Download
{
	public class AudioResultButton: MonoBehaviour
	{
        [SerializeField] private TMP_Text buttonName;
        [SerializeField] private TMP_Text buttonDesc;
        [SerializeField] public Animator loading;
        private string title;
        private string downloadURL;
        private string previewURL;
        private CancellationTokenSource previewCTS;
        public static AudioResultButton isPlaying;
        public static AudioResultButton playNext;

        public void Setup(string name, string description, string download, string preview, float duration)
        {
            title = name;
            buttonName.text = name.Truncate(30, "...") + " | " + TimeSpan.FromSeconds(duration).ToString("%m' m. '%s' s. ''%f' ms.'\"");
            buttonDesc.text = description;
            downloadURL = download;
            previewURL = preview;
        }

        public void Download()
        {
            AudioSearchManager.Instance.SelectSearchResult(title, downloadURL);
        }

        public void Preview()
        {
            if (isPlaying == this)
            {
                Stop();
                playNext = null;
                return;
            }

            if (isPlaying != null)
            {
                isPlaying.Stop();
                playNext = this;
                return;
            }

            isPlaying = this;
            playNext = null;
            Debug.Log("isPlaying was set to" + isPlaying);
            loading.SetTrigger("Play");

            previewCTS = new CancellationTokenSource();
            AudioSearchManager.Instance.PlayPreview(previewURL, previewCTS.Token);
        }

        private void PlayNext()
        {
            isPlaying = this;
            playNext = null;
            Debug.Log("isPlaying was set to" + isPlaying);
            loading.SetTrigger("Play");

            previewCTS = new CancellationTokenSource();
            AudioSearchManager.Instance.PlayPreview(previewURL, previewCTS.Token);
        }

        public void Stop()
        {
            Debug.Log(isPlaying + " was just stopped");
            previewCTS?.Cancel();
            previewCTS?.Dispose();
        }

        public void End()
        {
            Debug.Log(isPlaying + " ended");
            previewCTS?.Dispose();
            loading.SetTrigger("Stop");
            isPlaying = null;
            Debug.Log("isPlaying was set to null from " + this);
            if (playNext != null)
                playNext.PlayNext();
        }

        public void Cancel()
        {
            loading.SetTrigger("Stop");
            isPlaying = null;
            Debug.Log("isPlaying was set to null from " + this);
            if(playNext != null)
                playNext.PlayNext();

        }

        public override string ToString()
        {
            return title;
        }
    }
}