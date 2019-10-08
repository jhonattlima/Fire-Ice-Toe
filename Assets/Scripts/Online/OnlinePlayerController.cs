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
        if ((isServer && isLocalPlayer) || (!isServer && !isLocalPlayer))
        {
            this.playerMagic = 1;
            this.playerNumber = 1;
        }
        else if((!isServer && isLocalPlayer) || (isServer && !isLocalPlayer))
        { // Check if is the player 2, on client's only machine (need to pass this info to server)
            this.playerMagic = 2;
            this.playerNumber = 2;
            
        }
        if(!isServer && isLocalPlayer){
            CmdStartGame(Random.Range(1,3));  // START THE GAME WITH RANDOM PLAYER  
        }
    }

    [Command]
    private void CmdStartGame(int turn)
    {
        Debug.LogError("Tried to start game using Command with player number " + this.netId);
        RpcStartGame(turn);
    }

    [ClientRpc]
    private void RpcStartGame(int turn)
    {
        if(!isLocalPlayer) return;
        Debug.LogError("Tried to start the game using RPC with player number " + netId);
        lastPlayed = turn;
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
    }

    [Command]
    private void CmdCastMagic(int xpos, int ypos, int playerMagic)
    {
        RpcCastMagic(xpos, ypos, playerMagic);
    }

    [ClientRpc]
    private void RpcCastMagic(int xpos, int ypos, int playerMagic)
    {
        if(!isLocalPlayer) return;
        BoardManager.instance.setMagic(xpos, ypos, playerMagic);
        changeTurn();
        played = false;
    }

    private void changeTurn()
    {
        if(lastPlayed == 1) lastPlayed = 2;
        else lastPlayed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) return;
        if((isServer && lastPlayed == 2) || (!isServer && lastPlayed == 1))
        {
            if(BoardManager.instance != null)
            {
                if(BoardManager.instance.getHit())
                {
                    played = true;
                    CmdCastMagic(BoardManager.instance.lastMovementSet[0], BoardManager.instance.lastMovementSet[1], playerMagic);
                }
            }
        }
    }

    // public void startGame(int turn){ // Called only in server
    //     RpcStartGame(turn);
    // } 

    // [Command]
    // private void CmdStartGame(int turn){
    //     OnlineOrquestrator.startGame(turn, true);
    // }

    // [ClientRpc]
    // private void RpcStartGame(int turn){
    //     if(!isLocalPlayer) return;
    //     OnlineOrquestrator.startGame(turn, false);
    // }

    // public void changeTurn(int turn){ // Called only in server
    //     RpcChangeTurn(turn);
    // }

    // [Command]
    // public void CmdChangeTurn(int turn){
    //     OnlineOrquestrator.changeTurn(turn, true);
    // } 

    // [ClientRpc]
    // private void RpcChangeTurn(int turn){
    //     if(!isLocalPlayer) return;
    //     this.lastPlayed = turn;
    // }

    // public void castMagic(int xpos, int ypos, int playerMagic){ // Called only in server
    //     RpcUpdateBoard(xpos, ypos, playerMagic);
    // }

    // [Command]
    // private void CmdCastMagic(int xpos, int ypos, int playerMagic){
    //     OnlineOrquestrator.setPlay(xpos, ypos, playerMagic, true);
    // }

    // [ClientRpc]
    // private void RpcUpdateBoard(int xpos, int ypos, int playerMagic){
    //     if(!isLocalPlayer) return;
    //     OnlineOrquestrator.setPlay(xpos, ypos, playerMagic, false);
    //     OnlineOrquestrator.changeTurn(lastPlayed, false);
    //     //BoardManager.instance.setMagic(i,j,playerMagic);
    //     // int result = BoardManager.instance.isGameOver(BoardManager.instance.getBoard());
    //     // if( result != 0){
    //     //     if(result == this.playerMagic){
    //     //         // Finish game and present youwin screen
    //     //     } else if (result == 3){
    //     //         // Finish game and present draw screen
    //     //     } else {
    //     //         // Finish game and present lose screen
    //     //     }
    //     // }
    //     played = true;
    // }  
}
