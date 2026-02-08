using UnityEngine;
using TMPro;

public class DataNamer : MonoBehaviour
{

    [SerializeField] private TMP_Text errorField;
    [SerializeField] private TMP_InputField input;

    public void SetName(string name)
    {
        input.text = name;
    }

    public void DuplicateError(string oldName)
    {
        input.text = oldName;
        errorField.gameObject.SetActive(true);
        errorField.text = "This name is in use.";
    }

    public void NoError()
    {
        errorField.gameObject.SetActive(false);
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
