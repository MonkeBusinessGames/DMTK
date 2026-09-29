using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomDropdown : MonoBehaviour
{
    public GameObject options;

    public UnityEvent<CustomDropdownOption> optionSelected;

    public CustomDropdownOption defaultOption;

    public Image previewImage;

    public void OptionSelected(CustomDropdownOption option)
    {
        optionSelected.Invoke(option);
        options.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void ToggeSelector()
    {
        if(options.activeSelf)
        {
            options.SetActive(false);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(options);
            options.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (eventData.selectedObject != options)
        {
            options.SetActive(false);
            this.gameObject.SetActive(true);
        }
    }
}
