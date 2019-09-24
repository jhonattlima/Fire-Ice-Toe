using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private int[] inArray;

    private List<GameObject> buttons;

    void Start()
    {
        buttons = new List<GameObject>();
        // Destroys the buttons before creating new ones
        if (buttons.Count >0)
        {
            foreach (GameObject button in buttons)
                {
                Destroy(button.gameObject);
            }
        }
        foreach (int i in inArray)
        {
            // Spawn buttons
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            // Writes on the buttons
            button.GetComponent<ButtonListButton>().SetText("Game #" + i);

            // Parents the buttons in the template and don't let the buttons be placed using the world as reference.
            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }
    }

    //For now just print the button name
    public void ButtonClicked(string myTextString)
    {
        Debug.Log(myTextString);
    }
}
