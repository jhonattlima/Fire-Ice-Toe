using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlinePlayerController : NetworkBehaviour
{
    // Variables
    public static OnlinePlayerController localPlayer;
    public int playerMagic; // If it is fire or ice
    public int playerNumber; // If it is player 1 or 2
    public bool played = false;

    private void Awake()
    {   // Persist on scene change
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if(isLocalPlayer)
        {   // Get local player instance as a static object
            localPlayer = this;
        }
        if(isServer && isLocalPlayer)
        {   // Logic applied only to player 1
            playerMagic = GameManager.instance.playerMagic;
            OnlineOrquestrator.player1Magic = playerMagic;
            playerNumber = 1;
            CmdsetPlayer1Magic(playerMagic);
        }
        else if(!isServer && isLocalPlayer)
        {   // Logic applied only to player 2
            playerNumber = 2;
            CmdStartGame(Random.Range(1,3));   
        }
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        if((isServer && OnlineOrquestrator.turn == 2) || (!isServer && OnlineOrquestrator.turn == 1))
        {   // Logic for player if it is his turn
            if(BoardManager.instance != null && !played)
            {   // Logic if board already exists
                if(BoardManager.instance.getHit())
                {   // Prevent a fast player from playing twice and cast magic in all clients boards
                    played = true;
                    CmdCastMagic(BoardManager.instance.lastMovementSet[0], BoardManager.instance.lastMovementSet[1], playerMagic);
                }
            }
        }
    }

    [Command]
    private void CmdsetPlayer1Magic(int magic)
    {
        RpcSetPlayer1Magic(magic);
    }

    [ClientRpc]
    private void RpcSetPlayer1Magic(int magic)
    {   // Set player 1 magic in all machines
        OnlineOrquestrator.player1Magic = magic;
    }

    [Command]
    private void CmdStartGame(int turn)
    {   
        RpcStartGame(turn);
    }

    [ClientRpc]
    private void RpcStartGame(int turn)
    {   // Start the game in all machines
        if(isServer)
        {   // Get magic only from player 1 machine
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
    {   // Set the last played magic in all machines
        BoardManager.instance.setMagic(xpos, ypos, playerMagic);
        changeTurn();
        played = false;
    }

    private void changeTurn()
    {   // Check if the game didn't end yet, then change turn and turn message
        checkIfIsgameOver();
        if(OnlineOrquestrator.turn == 1) OnlineOrquestrator.turn = 2;
        else OnlineOrquestrator.turn = 1;
        BoardManager.instance.setTurnMessage(localPlayer.playerNumber);
    }
    
    private void checkIfIsgameOver()
    {   // Check if is game over
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

    private IEnumerator waitForPlayer1Magic()
    {   // Called by player 2. Wait for player 1 magic to be set in Online Orquestrator then pick player's 2 magic
        yield return new WaitUntil(() => OnlineOrquestrator.player1Magic != 0);
        if(!isServer && isLocalPlayer)
        {
            if(OnlineOrquestrator.player1Magic == 1) this.playerMagic = 2;
            else this.playerMagic = 1;
        }        
        SceneController.instance.changeScene(GameManager.instance.sceneBoard);
        yield return new WaitForSeconds(0.2f);
        BoardManager.instance.setTurnMessage(localPlayer.playerNumber);
    }
}
