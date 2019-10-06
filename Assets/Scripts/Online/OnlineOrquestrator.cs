using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineOrquestrator
{
    // VARIABLES
    // List of players (max 2)
    public static List<OnlinePlayerController> _playerList = new List<OnlinePlayerController>(); // ONLY SERVER WILL USE THIS
    public static int lastTurn; // LAST TURN THAT MUST BE SYNCED IN ALL

    public static void addPlayer(OnlinePlayerController player){ // ONLY SERVER CALL THIS
        _playerList.Add(player);
        Debug.Log("OnlineOrquestrator.addPlayer: New player added to Orquestrator.");
        Debug.Log("number odf players now: " + _playerList.Count);
    }

    public static void startGame(int getlastTurn, bool isServer){
        // Random turn
        lastTurn = getlastTurn;
        // Start the game
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
        if(isServer){
            foreach(var player in _playerList){
                player.updateTurn(getlastTurn);
            }
        }
    }

    public static void changeTurn(){
        if(lastTurn == 1){
            lastTurn = 2;
        } 
        else{
            lastTurn = 1;
        } 
        Debug.Log("Last turn: Player " + lastTurn);
        // Change Board panel text
        BoardManager.instance.textTurn.text =  "Turn: Player " + lastTurn;
    }

    public static void setPlay(int xpos, int ypos, int playerMagic, bool isServer){
        BoardManager.instance.setMagic(xpos, ypos, playerMagic);
        changeTurn();
        if(isServer){
            foreach(var player in _playerList){
                player.castMagic(xpos, ypos, playerMagic, lastTurn);
                player.updateTurn(lastTurn);
            }
        }
    }

    public static void endGame(){
        // Close connections, destroy objects and return to lobby.
    }
}