using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    public Button buttonMatchPrefab;
    private List<Button> buttons = new List<Button>();

    // Lan variables
    public GameObject panelLocalMatches;
    public GameObject lanListContent;
    public LanController lanController;
    public LanDiscovery lanDiscovery;
    public Text lanInputField;

    // Online variables
    public GameObject panelOnlineMatches;
    public GameObject buttonListOnline;
    public OnlineController onlineController;

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
        //lanController =  gameObject.AddComponent(typeof (LanController)) as LanController;
        //string[] names = {"name1", "name2", "name3"};
        //updateMatchesList(names);
        //lanDiscovery = new LanDiscovery();
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
                SFXPlayer.instance.Play(GameManager.instance.buttonConfirmation);
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
                GameManager.instance.multiplayerMode = true;
                GameManager.instance.lanMode = true;
                panelMainMenu.SetActive(false);
                panelLocalMatches.SetActive(true);
                //lanController.startController();
                lanDiscovery.listenMatches();
                startButtonsList();
                break;
            case "online":
                break;
            case "cancel":
                returnToLobby();
                break;
            case "create":
                if(GameManager.instance.multiplayerMode){
                    if(GameManager.instance.lanMode){
                        // Check if lan textbox is empty
                        if(string.IsNullOrEmpty(lanInputField.text)){
                            Debug.Log("Insert match name!");
                        } else {
                            //lanController.CreateMatch(lanInputField.text);
                            lanDiscovery.createMatch(lanInputField.text);
                            panelLocalMatches.SetActive(false);
                            panelWaitingPlayers.SetActive(true);
                        }
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

    private void returnToLobby(){
        buttons.Clear();
        lanDiscovery.StopBroadcast();
        GameManager.instance.multiplayerMode = false;
        panelLocalMatches.SetActive(false);
        //lanController?.stopController();
        Destroy(lanController);
        panelOnlineMatches.SetActive(false);
        Destroy(onlineController);
        panelWaitingPlayers.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    private void startButtonsList(){
        buttons.Clear();
        GameObject panel;
        //Debug.Log(matchesNameList);
        if(GameManager.instance.lanMode){
            panel = panelLocalMatches;
        } else {
            panel = panelOnlineMatches;
        }
        for (int i =0; i<GameManager.instance.maxMatches; i++){
            Button newButton = Instantiate(buttonMatchPrefab, panel.transform.position, panel.transform.rotation);
            newButton.transform.SetParent(lanListContent.transform, false);
            newButton.gameObject.SetActive(false);
            buttons.Add(newButton);
        }
    }

    // public void updateMatchesList(Dictionary<ConnInfo, float> dict){
    //     var keys = dict.Keys.ToList();
    //     foreach(var item in keys){
    //         Debug.Log(item.name);
    //     }
    // }

    // public void updateMatchesList(List<StoredData> storedDatas){
    //     int counter = 0;
    //     foreach(StoredData data in storedDatas){
    //         if(counter < GameManager.instance.maxMatches){
    //             buttons[counter].gameObject.GetComponentInChildren<Text>().text = data.data;
    //             buttons[counter].gameObject.GetComponent<ButtonMatchController>().data = data;
    //             buttons[counter].gameObject.SetActive(true);
    //             counter ++;
    //         }
    //     }
    //     while(counter < GameManager.instance.maxMatches){
    //         buttons[counter].gameObject.SetActive(false);
    //         counter ++;
    //     }
    // }
    
    public void updateMatchesList(Dictionary<ConnInfo, float> activeMatches){
        int counter = 0;  
        var keys = activeMatches.Keys.ToList();
        foreach(var match in keys)
        {
            if(counter < GameManager.instance.maxMatches){
                buttons[counter].gameObject.GetComponentInChildren<Text>().text = match.name;
                //buttons[counter].gameObject.GetComponent<ButtonMatchController>().matchName = Encoding.Unicode.GetString(match.broadcastData);
                //buttons[counter].gameObject.GetComponent<ButtonMatchController>().index = counter;
                buttons[counter].gameObject.SetActive(true);
                counter ++;
            }
        }
        while(counter <GameManager.instance.maxMatches){
            buttons[counter].gameObject.SetActive(false);
            counter ++;
        }
    }

    // public void updateMatchesList(string[] _matchNames){  
    //     for(int i =0; i<_matchNames.Length; i++){
    //         if(_matchNames[i] != null){
    //             buttons[i].gameObject.GetComponentInChildren<Text>().text = _matchNames[i];
    //             buttons[i].gameObject.GetComponent<ButtonMatchController>().matchName = _matchNames[i];
    //             buttons[i].gameObject.GetComponent<ButtonMatchController>().index = i;
    //             buttons[i].gameObject.SetActive(true);
    //         } else {
    //             buttons[i].gameObject.SetActive(false);
    //         }
    //     }
    // }

    IEnumerator callScene()
    {
        yield return new WaitForSeconds(1);
        bool turn = (Random.value > 0.5f);
        GameManager.instance.turn = turn;
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    }
}