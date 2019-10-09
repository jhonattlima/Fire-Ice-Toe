using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class UIManager : MonoBehaviour
{
    // General
    public static UIManager instance;
    public GameObject panelMainMenu;
    public GameObject panelGameMode;
    public GameObject panelOptions;
    public GameObject panelMagicSelection;
    public GameObject panelPvp;
    public GameObject panelWaitingPlayers;
    public GameObject panelEmptyRoomName;
    public Button buttonMatchPrefab;
    private List<Button> buttons = new List<Button>();

    // Multiplayer variables
    public GameObject panelLocalMatches;
    public GameObject panelLanListContent;
    public LanDiscovery lanDiscovery;
    public Text lanInputField;
    public OnlineDiscovery onlineDiscovery;

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
    }
    
    public void buttonEvent(string button)
    {
        switch(button){
            case "play":
                panelMainMenu.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelGameMode.SetActive(true);
                break;
            case "options":
                panelMainMenu.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelOptions.SetActive(true);
                break;
            case "easy":
                panelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Easy";
                SFXPlayer.instance.Play(Constants.BUTTON_CONFIRMATION);
                panelMagicSelection.SetActive(true);
                break;
            case "hard":
                panelMainMenu.SetActive(false);
                GameManager.instance.difficulty = "Impossible";
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelMagicSelection.SetActive(true);
                break;
            case "pvp":
                panelGameMode.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelPvp.SetActive(true);
                break;
            case "ModeBack":
                panelGameMode.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelMainMenu.SetActive(true);
                break;
            case "OptionsBack":
                panelOptions.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelMainMenu.SetActive(true);
                break;
            case "PvpBack":
                panelPvp.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelGameMode.SetActive(true);
                break;
            case "LocalBack":
                panelLocalMatches.SetActive(false);
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
                panelPvp.SetActive(true);
                break;
            case "quit":
                Application.Quit();
                break;
            case "fire":
                GameManager.instance.setPlayerMagic(1);
                SFXPlayer.instance.Play(GameManager.instance.charBurn);
                if(GameManager.instance.multiplayerMode == true){
                    switchToMultiplayerWaitingLobby();
                } else {
                    StartCoroutine(callScene());
                }
                break;
            case "ice":
                GameManager.instance.setPlayerMagic(2);
                SFXPlayer.instance.Play(GameManager.instance.charFreeze);
                if(GameManager.instance.multiplayerMode == true){
                    switchToMultiplayerWaitingLobby();
                } else {
                    StartCoroutine(callScene());
                }
                break;
            case "lan":
                GameManager.instance.multiplayerMode = true;
                GameManager.instance.lanMode = true;
                panelPvp.SetActive(false);
                panelLocalMatches.SetActive(true);
                startButtonsList();
                lanDiscovery.listenMatches();
                break;
            case "online":
                GameManager.instance.multiplayerMode = true;
                GameManager.instance.lanMode = false;
                panelPvp.SetActive(false);
                panelLocalMatches.SetActive(true);
                startButtonsList();
                onlineDiscovery.startListeningMatches();
                break;
            case "cancel":
                returnToLobby();
                break;
            case "ok":
                panelEmptyRoomName.SetActive(false);
                panelLocalMatches.SetActive(true);
                break;
            case "create":
                if(GameManager.instance.multiplayerMode)
                {
                    // Check if textbox is empty
                    if(string.IsNullOrEmpty(lanInputField.text)){
                        panelLocalMatches.SetActive(false);
                        panelEmptyRoomName.SetActive(true);
                    } else {
                        panelLocalMatches.SetActive(false);
                        panelMagicSelection.SetActive(true);
                    }
                    break;
                } else {
                    // Check if online textbox is empty
                    break;
                }
            case "cancelMatch":
                returnToLobby();
                break;
        }
    }

    private void switchToMultiplayerWaitingLobby(){
        panelMagicSelection.SetActive(false);
        panelWaitingPlayers.SetActive(true);
        if(GameManager.instance.lanMode){
            lanDiscovery.createMatch(lanInputField.text);
        } else {
            onlineDiscovery.createMatch(lanInputField.text);
        }
    }

    public void enterOnMatch(ButtonMatchController button){
        if(GameManager.instance.lanMode)
        {
            lanDiscovery.enterOnMatch(button);
        } 
        else
        {
            onlineDiscovery.enterOnMatch(button);
        } 
    }

    private void returnToLobby()
    {
        lanDiscovery.stopListeningMatches();
        onlineDiscovery.stopListeningMatches();
        buttons.Clear();
        GameManager.instance.multiplayerMode = false;
        panelWaitingPlayers.SetActive(false);
        panelLocalMatches.SetActive(false);
        panelMagicSelection.SetActive(false);
        panelOptions.SetActive(false);
        panelPvp.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    private void startButtonsList(){
        buttons.Clear();
        for (int i =0; i<GameManager.instance.maxMatches; i++)
        {
            Button newButton = Instantiate(buttonMatchPrefab, panelLocalMatches.transform.position, panelLocalMatches.transform.rotation);
            newButton.transform.SetParent(panelLanListContent.transform, false);
            newButton.gameObject.SetActive(false);
            buttons.Add(newButton);
        }
    }

    public void updateMatchesList(StoredData[] storedDatas) // Handle data from LanDiscovery
    {
        for (int i=0; i<storedDatas.Length; i++)
        {
            if(storedDatas[i] != null)
            {
                buttons[i].gameObject.GetComponentInChildren<Text>().text = storedDatas[i].data;
                buttons[i].gameObject.GetComponent<ButtonMatchController>().lanMatch = storedDatas[i];
                buttons[i].gameObject.SetActive(true);
            } 
            else 
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void updateMatchesList(MatchInfoSnapshot[] storedMatches) // Handle data from OnlineDiscovery
    {
        int counter = 0;
        if (storedMatches == null) return;
        foreach (MatchInfoSnapshot match in storedMatches)
        {
            if(match != null)
            {
                buttons[counter].gameObject.GetComponentInChildren<Text>().text = match.name;
                buttons[counter].gameObject.GetComponent<ButtonMatchController>().match = match;
                buttons[counter].gameObject.SetActive(true);
                counter ++;
            }
        }
        while (counter < GameManager.instance.maxMatches)
        {
            buttons[counter].gameObject.SetActive(false);
            counter ++;
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