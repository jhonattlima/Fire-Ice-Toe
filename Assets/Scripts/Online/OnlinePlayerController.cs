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
    public int lastPlayed = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start(){
        if(isServer && !isLocalPlayer || !isServer && isLocalPlayer){ // Check if is the player 2, on client's only machine (need to pass this info to server)
            this.playerMagic = 2;
            this.playerNumber = 2;
        } else if (isServer && isLocalPlayer) {
            this.playerMagic = 1;
            this.playerNumber = 1;
        }
        if(isServer){
            OnlineOrquestrator.addPlayer(this); // Only server will have the list of players
        }
        if(isLocalPlayer && !isServer){
            CmdStartGame(Random.Range(1,2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) return;
        if((isServer && lastPlayed == 2) || (!isServer && lastPlayed == 1)){
            if(BoardManager.instance.getHit()){
                played = true;
                CmdCastMagic(BoardManager.instance.lastMovementSet[0], BoardManager.instance.lastMovementSet[1], playerMagic);
            }
        }
    }

    public void startGame(int turn){
        RpcStartGame(turn);
    } 

    public void updateTurn(int turn){
        RpcUpdateTurn(turn);
    }

    [ClientRpc]
    private void RpcUpdateTurn(int turn){
        if(!isLocalPlayer) return;
        this.lastPlayed = turn;
    }

    [Command]
    private void CmdStartGame(int turn){
        OnlineOrquestrator.startGame(turn, true);
    }

    [ClientRpc]
    private void RpcStartGame(int turn){
        if(!isLocalPlayer) return;
        OnlineOrquestrator.startGame(turn, false);
    } 

    // Method only called by OnlineOrquestrator in server
    public void castMagic(int xpos, int ypos, int playerMagic, int lastPlayed){
        RpcUpdateBoard(xpos, ypos, playerMagic, lastPlayed);
    }

    [Command]
    private void CmdCastMagic(int xpos, int ypos, int playerMagic){
        OnlineOrquestrator.setPlay(xpos, ypos, playerMagic, true);
    }

    [ClientRpc]
    private void RpcUpdateBoard(int xpos, int ypos, int playerMagic, int lastPlayed){
        if(!isLocalPlayer) return;
        OnlineOrquestrator.setPlay(xpos, ypos, lastPlayed, false);
        //BoardManager.instance.setMagic(i,j,playerMagic);
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
        played = true;
    }  
}
