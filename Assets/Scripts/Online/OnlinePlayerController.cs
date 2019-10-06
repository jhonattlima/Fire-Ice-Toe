using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlinePlayerController : NetworkBehaviour
{
    // Variables
    public int playerMagic;
    public int playerNumber; // If it is player 1 or 2
    public bool played = false;

    // Update is called once per frame
    void Update()
    {
        // if(!isLocalPlayer) return;
        // if(OnlineOrquestrator.playerTurn == this.playerMagic && !played){
        //     if(BoardManager.instance.getHit()){
        //         played = true;
        //         CmdCastMagic(BoardManager.instance.lastMovementSet[0], BoardManager.instance.lastMovementSet[1],playerMagic);
        //     }
        // }
    }

    [Command]
    public void CmdAddPlayer(){
        RpcAddPlayer();
    }

    [Command]
    private void CmdCastMagic(int i, int j, int playerMagic){
        RpcUpdateBoard(i, j, playerMagic);
    }

    [Command]
    public void CmdStartgame(){
        //RpcStartGame();
    }

    [ClientRpc]
    private void RpcAddPlayer(){
        OnlineOrquestrator.addPlayer(this);
    }

    [ClientRpc]
    private void RpcUpdateBoard(int i, int j, int playerMagic){
        BoardManager.instance.setMagic(i,j,playerMagic);
        int result = BoardManager.instance.isGameOver(BoardManager.instance.getBoard());
        if( result != 0){
            if(result == this.playerMagic){
                // Finish game and present youwin screen
            } else if (result == 3){
                // Finish game and present draw screen
            } else {
                // Finish game and present lose screen
            }
        }
        OnlineOrquestrator.changeTurn(playerNumber);
    }

    [ClientRpc]
    private void RpcStartGame(int turn){

    }



    
}
