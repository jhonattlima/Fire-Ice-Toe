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
    
    void Start()
    {
        MusicPlayer.instance.Play(GameManager.instance.musicMainMenu);
    }
    
    public void buttonEvent(string button)
    {
        switch(button){
            case "easy":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Easy";
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                _PanelMagicSelection.SetActive(true);
                break;
            case "hard":
                _PanelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Impossible";
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                _PanelMagicSelection.SetActive(true);
                break;
            case "quit":
                Application.Quit();
                break;
            case "fire":
                GameManager.instance.setPlayerMagic(1);
                SFXPlayer.instance.Play(GameManager.instance.charBurn);
                StartCoroutine(callScene());
                break;
            case "ice":
                GameManager.instance.setPlayerMagic(2);
                SFXPlayer.instance.Play(GameManager.instance.charFreeze);
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
