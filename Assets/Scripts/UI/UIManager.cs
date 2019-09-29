using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    // Variables
    public static UIManager instance;
    public GameObject panelMainMenu;
    public GameObject panelMagicSelection;
    public GameObject panelLocalMatches;
    public GameObject panelOnlineMatches;
    public GameObject buttonMatchPrefab;
    public GameObject buttonListLan;
    public GameObject buttonListOnline;
    private List<GameObject> buttons = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MusicPlayer.instance.Play(GameManager.instance.musicMainMenu);
        //string[] names = {"name1", "name2", "name3"};
        //updateMatchesList(names);
    }
    
    public void buttonEvent(string button)
    {
        switch(button){
            case "easy":
                panelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Easy";
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelMagicSelection.SetActive(true);
                break;
            case "hard":
                panelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Impossible";
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelMagicSelection.SetActive(true);
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
            case "lan":
                panelMainMenu.SetActive(false);
                GameManager.instance.multiplayerMode = true;
                GameManager.instance.lanMode = true;
                panelLocalMatches.SetActive(true);
                break;
            case "online":
                panelMainMenu.SetActive(false);
                GameManager.instance.multiplayerMode = true;
                GameManager.instance.lanMode = false;
                panelOnlineMatches.SetActive(true);
                break;
            case "cancel":
                panelLocalMatches.SetActive(false);
                panelOnlineMatches.SetActive(false);
                GameManager.instance.multiplayerMode = false;
                panelMainMenu.SetActive(true);
                break;
        }
    }

    public void updateMatchesList(string[] matchesNameList){
        buttons.Clear();
        GameObject panel;
        if(GameManager.instance.lanMode){
            panel = panelLocalMatches;
        } else {
            panel = panelOnlineMatches;
        }
        for (int i =0; i<matchesNameList.Length; i++){
            GameObject newButton = Instantiate(buttonMatchPrefab, panel.transform.position, panel.transform.rotation);
            Debug.Log("Created button " + matchesNameList[i]);
            newButton.transform.SetParent(buttonListLan.transform, false);
            newButton.GetComponentInChildren<Text>().text = matchesNameList[i];
            newButton.GetComponent<ButtonMatchController>().name = matchesNameList[i];
            newButton.GetComponent<ButtonMatchController>().index = i;
            newButton.SetActive(true);
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