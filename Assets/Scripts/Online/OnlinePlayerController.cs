using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlinePlayerController : NetworkBehaviour
{
    // Variables
    public int playerMagic; // If it is fire or ice
    public int playerNumber; // If it is player 1 or 2
    public bool played = false;
    public bool inGame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start(){
        if(isServer){
            OnlineOrquestrator.addPlayer(this);
        }
        if(isLocalPlayer && isServer){ // Check if is the player that opened the room
            this.playerMagic = GameManager.instance.playerMagic;
            this.playerNumber = 1;
        }
        else if (isLocalPlayer && !isServer){ // Check if is the player 2, on client's only machine
            if(GameManager.instance.playerMagic == 1){
                this.playerMagic = 2;
            } else {
                this.playerMagic = 1;
            }
            this.playerNumber = 2;
            CmdStartgame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inGame){
            if(!isLocalPlayer) return;
            if(OnlineOrquestrator.playerTurn == this.playerNumber && !played){
                if(BoardManager.instance.getHit()){
                    played = true;
                    CmdCastMagic(BoardManager.instance.lastMovementSet[0], BoardManager.instance.lastMovementSet[1], playerMagic);
                }
            }
        }
    }

    [Command]
    public void CmdStartgame(){
        RpcStartGame(Random.Range(1, 2));
    }

    [ClientRpc]
    private void RpcStartGame(int turn){
        OnlineOrquestrator.startGame(turn);
    }  

    [Command]
    private void CmdCastMagic(int i, int j, int playerMagic){
        RpcUpdateBoard(i, j, playerMagic);
    }

    [ClientRpc]
    private void RpcUpdateBoard(int i, int j, int playerMagic){
        BoardManager.instance.setMagic(i,j,playerMagic);
        // int result = BoardManager.instance.isGameOver(BoardManager.instance.getBoard());
        // if( result != 0){
        //     if(result == this.playerMagic){
        //         // Finish game and present youwin screen
        //     } else if (result == 3){
        //         // Finish game and present draw screen
        //     } else {
        //         // Finish game and present lose screen
        //     }
        // }
        OnlineOrquestrator.changeTurn();
        played = true;
    }  

    // [Command]
    // public void CmdAddPlayer(){
    //     RpcAddPlayer(this);
    // }

    // [ClientRpc]
    // private void RpcAddPlayer(OnlinePlayerController player){
    //     OnlineOrquestrator.addPlayer(player);
    // }
}
