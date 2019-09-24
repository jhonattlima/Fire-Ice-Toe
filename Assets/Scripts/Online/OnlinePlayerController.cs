using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlinePlayerController : NetworkBehaviour
{
   /* // Variables
    public int playerMagic;
    public int playerNumber; // If it is player 1 or 2

    // Start is called before the first frame update
    void Start()
    {
        if(isServer){
            OnlineOrquestrator.addPlayer(this);
        }
    }

        // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) return;
        if(OnlineOrquestrator.playerTurn == this.playerMagic){
            if(BoardManager.instance.getHit()){
                if(this.playerNumber == 1){ // True for player 1, false for player 2
                    OnlineOrquestrator.playerTurn = 2;
                } else {
                    OnlineOrquestrator.playerTurn = 1;
                }
            }
        } 
    }

    [Command]
    private void castMagic(){
        // Send new board to onlineOrquestrator
    }

    [ClientRpc]
    private void updateBoard(BoardManager board){
        // Get updated board
        // Check if game ended, disconnecting and prompting the correct screen
    }*/
}
