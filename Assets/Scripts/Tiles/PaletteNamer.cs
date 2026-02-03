using UnityEngine;
using TMPro;

public class PaletteNamer : MonoBehaviour
{

    [SerializeField] TMP_Text errorField;
    [SerializeField] TMP_InputField input;

    public void SetName(string name)
    {
        input.text = name;
    }

    public void DuplicateError()
    {
        input.text = "";
        errorField.gameObject.SetActive(true);
        errorField.text = "This name is in use.";
    }

    public void NoError()
    {
        errorField.gameObject.SetActive(true);
        errorField.text = "This name is in use.";
    }

    public bool RequiredCheck()
    {
        if (input.text == "")
        {
            errorField.gameObject.SetActive(true);
            errorField.text = "A name is required.";
            return false;
        }
        else
            return true;
    }

}
