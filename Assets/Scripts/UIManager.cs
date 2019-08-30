using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Variables
    [SerializeField]
    private GameObject _PanelMainMenu;
    [SerializeField]
    private GameObject _PanelMagicSelection;
    [SerializeField]
    private GameManager _gameManagerPrefab;

    public void buttonEvent(Button button)
    {
        switch(button.name){
            case "Button_newGame":
                _PanelMainMenu.SetActive(false);
                _gameManagerPrefab.setDifficulty("Easy");
                _PanelMagicSelection.SetActive(true);
                break;
            case "Button_newGame2":
                _PanelMainMenu.SetActive(false);
                _gameManagerPrefab.setDifficulty("Impossible");
                _PanelMagicSelection.SetActive(true);
                break;
            case "Button_quit":
                Application.Quit();
                break;
            case "Button_fireMage":
                _gameManagerPrefab.setPlayerMagic(1);
                StartCoroutine(callScene());
                break;
            case "Button_iceMage":
                _gameManagerPrefab.setPlayerMagic(2);
                StartCoroutine(callScene());
                break;
        }
    }

    IEnumerator callScene()
    {
        yield return new WaitForSeconds(1);
        bool turn = (Random.value > 0.5f);
        _gameManagerPrefab.setTurn(turn); 
        _gameManagerPrefab.setBoardSize(3); // Future?
        SceneManager.LoadScene("Board 3x3", LoadSceneMode.Single);
    }
}
