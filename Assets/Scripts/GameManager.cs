using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    private int _playerMagic; // 1 = fire, 2= ice
    private int _aiMagic; // !playerMagic
    private string _difficulty; // Easy, Impossible
    private int _boardSize; // 3
    private bool _initialTurn; // true = player, false = AI
    private string _winner; // Player 1, AI or Draw

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // GETTERS AND SETTERS
    public int getPlayerMagic()
    {
        return this._playerMagic;
    }
    public void setPlayerMagic(int playerMagic)
    {
        this._playerMagic = playerMagic;
        if (playerMagic == 1)
        {
            _aiMagic = 2;
        }
        else
        {
            _aiMagic = 1;
        }
    }

    public int getAIMagic()
    {
        return this._aiMagic;
    }

    public string getDifficulty()
    {
        return this._difficulty;
    }

    public void setDifficulty(string difficulty)
    {
        this._difficulty = difficulty;
    }

    public bool getTurn()
    {
        return this._initialTurn;
    }

    public void setTurn(bool initialTurn)
    {
        this._initialTurn = initialTurn;
    }

    public int getBoardSize()
    {
        return _boardSize;
    }

    public void setBoardSize(int boardSize)
    {
        this._boardSize = boardSize;
    }

    public string getWinner()
    {
        return _winner;
    }

    public void setWinner(string winner)
    {
        this._winner = winner;
    }
}
