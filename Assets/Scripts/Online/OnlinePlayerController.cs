using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlinePlayerController : NetworkBehaviour
{
    private static OnlinePlayerController localPlayer;

    // Variables
    public int playerMagic; // If it is fire or ice
    public int playerNumber; // If it is player 1 or 2
    public bool played = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start(){
        if(isLocalPlayer)
        {
            localPlayer = this;
        }
        if ((isServer && isLocalPlayer))
        {
            this.playerMagic = GameManager.instance.playerMagic;
            OnlineOrquestrator.player1Magic = this.playerMagic;
            this.playerNumber = 1;
            CmdsetPlayer1Magic(this.playerMagic);
        }
        else if((!isServer && isLocalPlayer) || (isServer && !isLocalPlayer))
        { // Check if is the player 2, on client's only machine (need to pass this info to server)
            this.playerNumber = 2;   
        }
        if(!isServer && isLocalPlayer){
            CmdStartGame(Random.Range(1,3));  // START THE GAME WITH RANDOM PLAYER  
        }
    }

    [Command]
    private void CmdsetPlayer1Magic(int magic){
        RpcSetPlayer1Magic(magic);
    }

    [ClientRpc]
    private void RpcSetPlayer1Magic(int magic){
        OnlineOrquestrator.player1Magic = magic;
        Debug.LogError("OnlineOrquestrator player1Magic set to " + magic);
    }

    [Command]
    private void CmdStartGame(int turn)
    {
        //Debug.LogError("Tried to start game using Command with player number " + this.netId);
        RpcStartGame(turn);
    }

    [ClientRpc]
    private void RpcStartGame(int turn)
    {
        //if(!isLocalPlayer) return;
        //Debug.LogError("Tried to start the game using RPC with player number " + netId);
        if(isServer){
            CmdsetPlayer1Magic(OnlineOrquestrator.player1Magic);
        }
        OnlineOrquestrator.turn = turn;
        StartCoroutine(waitForPlayer1Magic());
    }

    [Command]
    private void CmdCastMagic(int xpos, int ypos, int playerMagic)
    {
        RpcCastMagic(xpos, ypos, playerMagic);
    }

    [ClientRpc]
    private void RpcCastMagic(int xpos, int ypos, int playerMagic)
    {
        //if(!isLocalPlayer) return;
        BoardManager.instance.setMagic(xpos, ypos, playerMagic);
        changeTurn();
        played = false;
    }

    private void changeTurn()
    {
        checkIfIsgameOver();
        if(OnlineOrquestrator.turn == 1) OnlineOrquestrator.turn = 2;
        else OnlineOrquestrator.turn = 1;
        //Debug.LogError("OnlineOrquestrator turn " + OnlineOrquestrator.turn);
        setTurnMessage();
    }

    private void setTurnMessage()
    {
        string message = null;
        if(localPlayer.playerNumber != OnlineOrquestrator.turn)
        {
            message = "Your Turn!";
        } else 
        {
            message = "Opponent's turn!";
        }
        if(BoardManager.instance)
        {
            BoardManager.instance.textTurn.text = message;
        }
    }
    
    void Update()
    {
        if(!isLocalPlayer) return;
        if((isServer && OnlineOrquestrator.turn == 2) || (!isServer && OnlineOrquestrator.turn == 1))
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

    private IEnumerator waitForPlayer1Magic()
    {
        yield return new WaitUntil(() => OnlineOrquestrator.player1Magic != 0);
        if(!isServer && isLocalPlayer)
        {
            if(OnlineOrquestrator.player1Magic == 1) this.playerMagic = 2;
            else this.playerMagic = 1;
        }        
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
        yield return new WaitForSeconds(0.2f);
        setTurnMessage();
    }

    private void checkIfIsgameOver(){
        int result = BoardManager.instance.isGameOver(BoardManager.instance.getBoard());
        if(result != 0)
        {

            if (result == 3)
            {
                // Draw
                GameManager.instance.winner = "Draw"; 
            }
            else if(result == localPlayer.playerMagic)
            {
                // Player won
                GameManager.instance.winner = "Player";
            }
            else
            {
                //Player Lost
                GameManager.instance.winner = "Other";
            }
            SceneController.instance.changeScene(GameManager.instance.sceneGameOver);
            Destroy(gameObject);
        }
    }
}
