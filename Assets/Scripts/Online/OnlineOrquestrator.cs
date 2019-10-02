using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Start the game
        GameManager.instance.mode = "online";
        randomTurn();
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    }

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
}