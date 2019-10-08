using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineOrquestrator
{
    // // VARIABLES
    // // List of players (max 2)
    // public static List<OnlinePlayerController> _playerList = new List<OnlinePlayerController>(); // ONLY SERVER WILL USE THIS
    // public static int lastTurn; // LAST TURN THAT MUST BE SYNCED IN ALL

    // public static void addPlayer(OnlinePlayerController player){ // ONLY SERVER CALL THIS
    //     _playerList.Add(player);
    //     Debug.Log("OnlineOrquestrator.addPlayer: New player added, number os players now: " + _playerList.Count);
    // }

    // public static void startGame(int getlastTurn, bool isServer){
    //     // Random turn
    //     lastTurn = getlastTurn;
    //     // Start the game
    //     //Debug.LogError("Reached start game");
    //     SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    //     if(isServer){
    //         foreach(var player in _playerList){
    //             player.startGame(getlastTurn);
    //             player.changeTurn(getlastTurn);
    //         }
    //     }
    // }

    // public static void changeTurn(int turn, bool isServer){
    //     if(turn == 1) lastTurn = 2 ;
    //     else lastTurn = 1;
    //     Debug.LogError("Last turn: Player " + lastTurn);
    //     // Change Board panel text
    //     BoardManager.instance.textTurn.text =  "Turn: Player " + lastTurn;
    //     if(isServer){
    //         foreach(var player in _playerList){
    //             player.changeTurn(lastTurn);
    //         }
    //     }
    // }

    // public static void setPlay(int xpos, int ypos, int playerMagic, bool isServer){ 
    //     BoardManager.instance.setMagic(xpos, ypos, playerMagic);
    //     if(isServer){
    //         foreach(var player in _playerList){
    //             player.castMagic(xpos, ypos, playerMagic);
    //         }
    //     }
    // }

    // public static void endGame(){
    //     // Close connections, destroy objects and return to lobby.
    // }
}