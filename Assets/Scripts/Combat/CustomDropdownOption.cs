using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomDropdownOption : MonoBehaviour
{
    public Sprite icon;
    public Image backgroundImage;
    public Image iconImage;
    public TMP_Text text;
    public string label;
    public CustomDropdown dropdown;
    public Color backgroundColor;
    [SerializeField] private Color hoverColor;

    public CustomDropdownOption(CustomDropdown parentDropdown, Sprite iconSprite, string labelText, Color color)
    {
        dropdown = parentDropdown;
        iconImage.sprite = icon = iconSprite;
        text.text = label = labelText;
        backgroundImage.color = backgroundColor = color;
        hoverColor = new Color(backgroundColor.r + .05f, backgroundColor.g + .05f, backgroundColor.b + .05f, backgroundColor.a);
    }

    [ContextMenu("Update Option")]
    private void UpdateIcon()
    {
        backgroundImage.color = backgroundColor;
        iconImage.sprite = icon;
        text.text = label;
        hoverColor = new Color(backgroundColor.r + .05f, backgroundColor.g + .05f, backgroundColor.b + .05f, backgroundColor.a);
    }

    public void StartHover()
    {
        backgroundImage.color = hoverColor;
    }

    public void EndHover()
    {
        backgroundImage.color = backgroundColor;
    }

    public void SelectOption()
    {
        dropdown.OptionSelected(this);
        dropdown.gameObject.SetActive(true);
    }


}
