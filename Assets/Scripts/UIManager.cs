using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Variables
    public GameObject _PanelMainMenu;
    public GameObject _PanelMagicSelection;

    public void buttonEvent(Button button)
    {
        switch(button.name){
            case "Button_newGame":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Easy";
                _PanelMagicSelection.SetActive(true);
                break;
            case "Button_newGame2":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Impossible";
                _PanelMagicSelection.SetActive(true);
                break;
            case "Button_quit":
                Application.Quit();
                break;
            case "Button_fireMage":
                GameManager.instance.setPlayerMagic(1);
                StartCoroutine(callScene());
                break;
            case "Button_iceMage":
                GameManager.instance.setPlayerMagic(2);
                StartCoroutine(callScene());
                break;
        }
    }

    IEnumerator callScene()
    {
        yield return new WaitForSeconds(1);
        bool turn = (Random.value > 0.5f);
        GameManager.instance.turn = turn;
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    }
}
