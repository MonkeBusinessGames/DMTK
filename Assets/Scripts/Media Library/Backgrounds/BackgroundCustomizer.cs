using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Windows.Forms;

namespace Assets.Scripts.Media_Library.Backgrounds
{
    public class NewMonoBehaviour : MonoBehaviour
    {

        [SerializeField] Image dmBackground;
        [SerializeField] Image playerBackground;
        [SerializeField] Toggle stretcher;
        [SerializeField] FlexibleColorPicker colorPicker;

        [SerializeField] private GameObject backgroundCustomizer;


        public void OpenBackgroundCustomizer()
        {
            backgroundCustomizer.SetActive(true);
        }
        public void CloseBackgroundCustomizer()
        {
            backgroundCustomizer.SetActive(true);
        }

        public void SetColor(Color newColor)
        {
            dmBackground.color = playerBackground.color = newColor;
        }

        public void ResetBackground()
        {
            stretcher.SetIsOnWithoutNotify(true);
            StretchToFit(true);
            SetColor(Color.white);
            colorPicker.SetColor(Color.white);
        }

        public void StretchToFit(bool stretch)
        {
            dmBackground.preserveAspect = playerBackground.preserveAspect = !stretch;
        }
    }
}