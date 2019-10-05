using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OnlineOrquestrator
{
    // VARIABLES
    // List of players (max 2)
    private static List<OnlinePlayerController> _playerList = new List<OnlinePlayerController>();
    public static int playerTurn;

    public static void addPlayer(OnlinePlayerController player){
        _playerList.Add(player);
    }

    private static void startGame(int newplayerTurn){
        // Define the second player magic
        if(_playerList[0].playerMagic == 1){
            _playerList[1].playerMagic = 2;
        } else {
            _playerList[1].playerMagic = 1;
        }
        playerTurn = newplayerTurn;
        // Start the game
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    }

    public static void changeTurn(int playerNumber){
        if(playerTurn == 1) playerTurn = 2;
        else playerTurn = 1;
        _playerList[playerNumber-1].played = false;
    }

    private static void setTurn(int turn){
        playerTurn = turn;
    }
}