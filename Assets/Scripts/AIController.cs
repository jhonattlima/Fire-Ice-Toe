using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Variables
    private GameManager _gameManagerPrefab; // Game manager imported to use variables
    [SerializeField]
    private BoardManager _boardManagerPrefab; // Board manager imported to use methods
    private int _magic; // AI magic (not player selected magic)
    private int line, column; // For use in minimax, getting the best line, column to move

    private void Start()
    {
        _gameManagerPrefab = GameObject.Find("GameManager").GetComponent<GameManager>();
        this._magic = _gameManagerPrefab.getAIMagic();    
    }

    // AI play
    public bool play() {
        if (_gameManagerPrefab.getDifficulty().Equals("Easy"))
        {
            dummyPlay(_boardManagerPrefab.getBoard()); // Check the dummy play and do it
        } else
        {
            bestPlay(_boardManagerPrefab.getBoard()); // Check the best play and do it
        }
        return true;
    }

    // Use random play
    private void dummyPlay(int[,] board)
    {
        int line = Random.Range(0, 3);
        int column = Random.Range(0, 3);

        while (board[line, column] != 0)
        {
            line = Random.Range(0, 3);
            column = Random.Range(0, 3);
        }
        _boardManagerPrefab.setMagic(line, column, _magic); // AI plays in the defined line and column
        Debug.Log("AI dummy played at line: " + line + " Column: " + column);
    }

    // Uses MinMax to detect best play
    private void bestPlay(int[,] board)
    {
        int oldValue = -999999; // Variables to store the values

        // Test all possible plays
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                // Check if the space is empty
                if (board[i, j] == 0)
                {
                    int[,] copyBoard = (int[,])board.Clone(); // Create a new string board that's a copy of entered parameter board
                    copyBoard[i, j] = _magic;
                    int newValue = miniMax(copyBoard, false);
                    if (newValue > oldValue)
                    {
                        // Line and column to do the movement (this will be constantly updated until the end of testing all the ways
                        line = i;
                        column = j;
                        oldValue = newValue;
                    }
                }
            }
        }
        _boardManagerPrefab.setMagic(line, column, _magic); // AI plays in the defined line and column
        Debug.Log("AI smartly played at line: " + line + " Column: " + column);
    }

    // MiniMax
    private int miniMax(int[,] miniboard, bool turn)
    {
        if (_boardManagerPrefab.isGameOver(miniboard) != 0) // Check if game ended
        {
            int result = calcScore(miniboard);
            return result;
        }
        else // If not game over
        {
            int value;
            if (!turn) // Simulating player 1 turn
            {
                value = 10000;
            }
            else
            {
                value = -10000;
            }

            for (int i = 0; i < miniboard.GetLength(0); i++) // Test all available spaces
            {
                for (int j = 0; j < miniboard.GetLength(0); j++)
                {
                    if (miniboard[i, j] == 0)
                    {
                        int[,] copyBoard = (int[,])miniboard.Clone(); // Create a new int board that's a copy of entered parameter board
                        if (!turn) // Simulating player 1 turn
                        {
                            copyBoard[i, j] = _gameManagerPrefab.getPlayerMagic();
                            value = Mathf.Min(value, miniMax(copyBoard, !turn));
                        }
                        else if (turn) // Simulating AI turn
                        {
                            copyBoard[i, j] = _magic;
                            value = Mathf.Max(value, miniMax(copyBoard, !turn));
                        }
                    }
                }
            }
            return value;
        }
    }

    // Function to calculate score
    private int calcScore(int[,] board)
    {
        if(_boardManagerPrefab.isGameOver(board) == this._magic)
        {
            return +1; // AI won
        }
        else if (_boardManagerPrefab.isGameOver(board) == this._gameManagerPrefab.getPlayerMagic())
        {
            return -1; // Player 1 won
        } 
        return 0; //Draw
    }

    //Getters and setters 
    public void setMagic(int magic)
    {
        this._magic = magic;
    }
}
