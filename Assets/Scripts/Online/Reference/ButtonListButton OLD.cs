using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour
{
    [SerializeField]
    private Text myText;
    [SerializeField]
    private ButtonListControl buttonControl;

    private string myTextString;

    // Creates the text for the buttons
    public void SetText(string textString)
    {
        myTextString = textString;
        myText.text = textString;
    }

    // Sends the button command to ButtonListControl
    public void OnClick()
    {
        buttonControl.ButtonClicked(myTextString);
    }
}