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
            case "ButtonEasyMode":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Easy";
                _PanelMagicSelection.SetActive(true);
                break;
            case "ButtonHardMode":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Impossible";
                _PanelMagicSelection.SetActive(true);
                break;
            case "ButtonQuit":
                Application.Quit();
                break;
            case "ButtonFireMage":
                GameManager.instance.setPlayerMagic(1);
                StartCoroutine(callScene());
                break;
            case "ButtonIceMage":
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
