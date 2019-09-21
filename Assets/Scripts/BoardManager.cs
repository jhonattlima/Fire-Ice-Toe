using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Variables 
    private Space[,] _board; // Elements board
    private int[,] _intBoard; // Characters board for tracking
    private int _boardSize;
    public Space _spacePrefab; // Space prefab to be instantiated during board creation
    public GameObject _icePrefab; // Ice prefab to be instantiated during board update
    public GameObject _firePrefab; // Fire prefab to be instantiated during board update

    // Start is called before the first frame update - WORKING
    void Start()
    {
        _boardSize = GameManager.instance.boardSize;
        createBoard();
    }

    // For debuging
    public void printBoard(int[,] board)
    {
        print("Printing the board...");
        for (int i = 0; i < GameManager.instance.boardSize; i++)
        {
            for (int j = 0; j < GameManager.instance.boardSize; j++)
            {
                if(board[i,j] != 0)
                {
                    Debug.Log("IntBoard " + i + "," + j + "=" + board[i, j]);
                }
            }
        }
    }

    //Check if the spaces in the board are clicked - WORKING
    public bool getHit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Space")
                {
                    Space space = hit.collider.GetComponent<Space>();
                    if (!space.getMagic()){
                        for (int i = 0; i < GameManager.instance.boardSize; i++)
                        {
                            for (int j = 0; j < GameManager.instance.boardSize; j++)
                            {
                                if(space == _board[i, j])
                                {
                                    return setMagic(i, j, GameManager.instance.playerMagic);
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    // Create the board - WORKING
    private void createBoard()
    {
        _board = new Space[_boardSize, _boardSize];
        _intBoard = new int[_boardSize, _boardSize];
        // Create a board for elements and one for tracking
        for (int i = 0; i < _boardSize; i++)
        {
            for (int j = 0; j < _boardSize; j++)
            { 
                Space space = Instantiate(_spacePrefab, Vector3.zero, transform.rotation);
                space.transform.parent = this.transform;
                space.transform.position = new Vector3(space.transform.position.x + 3.5f * (i - 1), space.transform.position.y + 3.5f * (j - 1), space.transform.position.z);
                _board[i, j] = space;
                _intBoard[i, j] = 0; // 0 means for empty
            }
        }
    }

    // Check if the game is over - WORKING
    public int isGameOver(int[,] board){
        // check the lines
        int lineIce = 0, lineFire = 0, columnIce = 0, columnFire = 0, diagonalIce = 0, diagonalFire = 0, index = 0;

        for(int i = 0; i < _boardSize; i++){
            for(int j = 0; j < _boardSize; j++){
                // Check the lines
                if(board[i,j] == 2) lineIce++;
                else if(board[i,j] == 1) lineFire++;

                // Check the columns
                if(board[j,i] == 2) columnIce++;
                else if(board[j,i] == 1) columnFire++;

                // Check the diagonal
                if (i == j && board[i, j] == 2) diagonalIce++;
                else if (i == j && board[i, j] == 1) diagonalFire++;
            }
            if (lineFire == _boardSize || columnFire == _boardSize || diagonalFire == _boardSize)
            {
                return 1;
            }
            if (lineIce == _boardSize || columnIce == _boardSize || diagonalIce == _boardSize)
            {
                return 2;
            }
            lineIce = 0;
            lineFire = 0;
            columnIce = 0; 
            columnFire = 0;
        }

        // Check for counterDiagonal
        diagonalFire = 0;
        diagonalIce = 0;
        for(int j = _boardSize - 1; j >= 0; j--)
        {
            if(board[index, j] == 2) diagonalIce++;
            else if (board[index, j] == 1) diagonalFire++;
            if (index < _boardSize)
            {
                index++;
            }
        }
        if (diagonalFire == _boardSize)
        {
            return 1;
        }
        if (diagonalIce == _boardSize)
        {
            return 2;
        }

        // Check for draw
        if(isFull(board)){
            return 3; // Draw
        }
        return 0; // Game not ended yet
    }

    //Check if the board is completely filled - WORKING
    private bool isFull(int[,] board){
        for(int i = 0; i < board.GetLength(0); i++){
            for(int j = 0; j < board.GetLength(0); j++){
                if(board[i, j] == 0){
                    return false;
                }
            }
        }
        return true;
    }

    // Getters and Setters - WORKING
    public int[,] getBoard() {
        return this._intBoard;
    }

    public int getBoardSize()
    {
        return this._boardSize;
    }

    public bool setMagic(int i, int j, int magic)
    {
        if(magic == 1)
        {
            Space space = _board[i, j];
            SFXPlayer.instance.Play(GameManager.instance.SFXFire);
            GameObject fire = Instantiate(_firePrefab, space.transform);
            space.setMagic(fire);
            _intBoard[i, j] = 1;
            return true;
        } else if (magic == 2)
        {
            Space space = _board[i, j];
            SFXPlayer.instance.Play(GameManager.instance.SFXIce);
            GameObject ice = Instantiate(_icePrefab, space.transform);
            space.setMagic(ice);
            _intBoard[i, j] = 2;
            return true;
        }
        return false;
    }
}
