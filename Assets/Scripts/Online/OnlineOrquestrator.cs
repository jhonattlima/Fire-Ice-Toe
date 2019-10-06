using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineOrquestrator
{
   /* // VARIABLES
    // List of players (max 2)
    // ====================================== Check with teacher how this will be syncronized
    private static List<OnlinePlayerController> _playerList = new List<OnlinePlayerController>();
    public static int playerTurn;
    public static BoardManager gameBoard;

    public static void addPlayer(OnlinePlayerController player){
        _playerList.Add(player);
<<<<<<< HEAD
        Debug.Log("OnlineOrquestrator.addPlayer: New player added to Orquestrator.");
        Debug.Log("number odf players now: " + _playerList.Count);
    }

    public static void startGame(int newplayerTurn){
        // Random turn
        playerTurn = newplayerTurn;
=======
        if(_playerList.Count >= 2){
            startGame();
        }
    }

    private static void startGame(){
        // Define the second player magic
        if(GameManager.instance.playerMagic == 1){
            _playerList[1].playerMagic = 2;
        } else {
            _playerList[1].playerMagic = 1;
        }
>>>>>>> 53d64bd729350ac80c8d4dc2095f9c2865628a43
        // Start the game
        GameManager.instance.mode = "online";
        randomTurn();
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
        foreach(OnlinePlayerController player in _playerList){
            player.inGame = true;
            //Debug.Log("OnlineOrquestrator.startGame: Changed player " +player.playerNumber+ " in game state to true.");
        }
    }

<<<<<<< HEAD
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
=======
    public static void updateBoard(){
        //Check if game ended
        int gameChecker = BoardManager.instance.isGameOver(BoardManager.instance.getBoard());
        if(gameChecker != 0){
            // Deal with winner, loser and draw
        } else {
            // if(isServer){
            //     foreach(var player in _playerList){
            //         p
            //     }
            // }
        }
    }

    private static void randomTurn(){
        playerTurn = Random.Range(1, 2);
    }*/
>>>>>>> 53d64bd729350ac80c8d4dc2095f9c2865628a43
}