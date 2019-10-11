using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public static class OnlineOrquestrator
{
    public static int turn = 0; // Global reference for turn
    public static int player1Magic = 0; // Global reference for player 1 magic type

    public static void clear()
    { // Clear this class to start again
        turn = 0;
        player1Magic = 0;
    }

    public static void restartOnlineGame()
    {
        try
        {
            string sceneMainMenu = GameManager.instance.sceneMainMenu; 
            if(NetworkController.singleton.matchInfo != null) NetworkController.Match.DestroyMatch(NetworkController.singleton.matchInfo.networkId, 
                NetworkController.singleton.matchInfo.domain, OnMatchDestroy);
            NetworkController.singleton.StopMatchMaker();
            GameObject aux = NetworkController.singleton.gameObject; 
            NetworkController.Shutdown();
            GameObject.Destroy(aux);
            GameObject.Destroy(GameManager.instance.gameObject);
            OnlineOrquestrator.clear();
            SceneController.instance.changeScene(sceneMainMenu);
        }
        catch (Exception e)
        {
            Debug.Log("Handled Error: " + e.Message);
        }
    }

    public static void cancelOnlineMatch()
    {   try
        {
            if(NetworkController.singleton.matchInfo != null) NetworkController.Match.DestroyMatch(NetworkController.singleton.matchInfo.networkId, 
            NetworkController.singleton.matchInfo.domain, OnMatchDestroy);
            NetworkController.singleton.StopMatchMaker();
            GameObject aux = NetworkController.singleton.gameObject; 
            GameObject.Destroy(GameManager.instance.gameObject);
            NetworkController.Shutdown();
            GameObject.Destroy(aux);
            OnlineOrquestrator.clear();
            GameObject.Destroy(UIManager.instance.gameObject);
            SceneController.instance.changeScene(Constants.SCENE_MENU);
        } 
        catch (Exception e)
        {
            Debug.Log("Handled Error: " + e.Message);
        }
    }

    public static void OnMatchDestroy(bool success, string extendedInfo)
    {
        // Destroy match dependency
    }
}