using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineOrquestrator
{
    // VARIABLES
    // List of players (max 2)
    private static List<OnlinePlayerController> _playerList = new List<OnlinePlayerController>();
    public static int playerTurn;

    public static void addPlayer(OnlinePlayerController player){
        _playerList.Add(player);
        Debug.Log("OnlineOrquestrator.addPlayer: New player added to Orquestrator.");
        Debug.Log("number odf players now: " + _playerList.Count);
    }

    public static void startGame(int newplayerTurn){
        // Random turn
        playerTurn = newplayerTurn;
        // Start the game
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
        foreach(OnlinePlayerController player in _playerList){
            player.inGame = true;
            //Debug.Log("OnlineOrquestrator.startGame: Changed player " +player.playerNumber+ " in game state to true.");
        }
    }

    public static void changeTurn(int playerNumber){
        if(playerTurn == 1){
            playerTurn = 2;
        } 
        else{
            playerTurn = 1;
        } 
        _playerList[playerNumber-1].played = false;
        Debug.Log("Changed turn to " + playerTurn);
        Debug.Log("Changed player " + _playerList[playerNumber-1].playerNumber + " played value to false." );

        // Change Board panel text
        BoardManager.instance.textTurn.text =  "Turn: Player " + playerTurn;
    }
}