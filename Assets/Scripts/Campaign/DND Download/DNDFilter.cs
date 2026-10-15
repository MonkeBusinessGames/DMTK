using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Campaign.DND_Download
{
    public class DNDFilter : MonoBehaviour
    {
        [SerializeField] DNDSearchManager searchManager;
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] TMP_Text dropdownLabel;
        [SerializeField] string filterLabel;
        [SerializeField] List<string> values;
        [SerializeField] List<string> keys;


        [ContextMenu("Update Filter UI")]
        public void UpdateDropdown()
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(values);

            dropdownLabel.text = filterLabel;
        }

        public void UpdateFilters()
        {
            if (dropdown.value == 0)
                searchManager.RemoveFilter(filterLabel);

            string returnValue = filterLabel + "=";

            // Get the mask value representing all selections
            int selectionMask = dropdown.value;

            // Loop through all available options in the dropdown
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                // Shift the bit to the current index position and check if it is active
                if ((selectionMask & (1 << i)) != 0)
                {
                    returnValue += keys[i] + ",";     
                }
            }
            returnValue.TrimEnd(",");
            searchManager.UpdateFilters(filterLabel, returnValue);
        }

    }
}