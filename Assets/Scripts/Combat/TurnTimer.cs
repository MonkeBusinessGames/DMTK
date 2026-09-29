using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Assets.Scripts.Combat
{
    public class TurnTimer : MonoBehaviour
    {
        [SerializeField] private GameObject timerSelector;
        [SerializeField] private GameObject timerButton;
        [SerializeField] private Image timerDisplay;
        [SerializeField] private TMP_Text timerDisplayText;
        [SerializeField] private TMP_InputField timerMinutesInput;
        [SerializeField] private TMP_InputField timerSecondsInput;

        public bool trackTime;
        private float timerCount = 180f;
        private float timerMax = 180f;


        private void Start()
        {
            float minute = Mathf.FloorToInt(timerCount / 60);
            timerMinutesInput.text = minute.ToString();
            timerSecondsInput.text = (timerCount - (minute*  60) ).ToString();
            ResetTimer();
            trackTime = false;
        }
        public void ResetTimer()
        {
            timerCount = timerMax;
            timerDisplay.color = Color.white;
            SetTimeText();
        }

        private void Update()
        {
            if (trackTime)
            {
                timerCount -= UnityEngine.Time.deltaTime;
                SetTimeText();
            }
        }

        private void SetTimeText()
        {   
            if(timerCount < 0)
            {
                timerDisplay.color = Color.softRed;
            }
            else if (timerCount < 60)
            {
                timerDisplay.color = Color.softYellow;
            }
            if(timerCount < 0)
            {

                timerDisplayText.text = "-" + TimeSpan.FromSeconds(timerCount).ToString(@"mm\:ss");
                return;
            }

            timerDisplayText.text = TimeSpan.FromSeconds(timerCount).ToString(@"mm\:ss");
        }

        public void UpdateTimer()
        {
            if (timerSecondsInput.text == "")
                timerSecondsInput.text = "00";
            if (timerMinutesInput.text == "")
                timerMinutesInput.text = "00";
            float seconds = float.Parse(timerSecondsInput.text);
            float minutes = float.Parse(timerMinutesInput.text);  
            if(seconds >= 60 || seconds < 0)
            {
                timerSecondsInput.text = "59";
                seconds = 59;
            }
            if (minutes >= 60 || minutes < 0)
            {
                timerMinutesInput.text = "59";
                minutes = 59;
            }
            timerMax = minutes * 60 + seconds;
            
            ResetTimer();
        }

        public void TimerOn()
        {
            timerSelector.SetActive(true);
            timerButton.SetActive(false);
            timerDisplay.gameObject.SetActive(true);
        }
        public void TimerOff()
        {
            timerSelector.SetActive(false);
            timerButton.SetActive(true);
            timerDisplay.gameObject.SetActive(false);
        }
        public void ToggleTimer()
        {
           trackTime = !trackTime;
        }

    }
}