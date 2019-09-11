using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orquestrator : MonoBehaviour
{
    // Variables
    private bool _turn;
    public BoardManager boardManagerPrefab;
    public AIController aiPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _turn = GameManager.instance.turn;
        Debug.Log("Orquestrator: Player magic is: " + GameManager.instance.playerMagic);
        Debug.Log("Orquestrator: AI magic is: " + GameManager.instance.aiMagic);
        Debug.Log("Orquestrator: Turn is: " + GameManager.instance.turn);
        Debug.Log("Orquestrator: BoardSize is: " + GameManager.instance.boardSize);
        Debug.Log("Orquestrator: Difficulty is: " + GameManager.instance.difficulty);
    }

    // Update is called once per frame - WORKING
    void Update()
    {
        checkTurn();
    }

    private void checkTurn()
    {
        if (_turn)
        { // Player turn = true
            if (boardManagerPrefab.getHit())
            {
                _turn = false;
            }
        }
        else
        {
            if (aiPrefab.play())
            {
                _turn = true;
            }
        }
        if (boardManagerPrefab.isGameOver(boardManagerPrefab.getBoard()) != 0)
        {
            GameOver();
        }
    }

    // Calls Game over screen
    private void GameOver()
    {
        if(boardManagerPrefab.isGameOver(boardManagerPrefab.getBoard()) == GameManager.instance.playerMagic)
        {
            GameManager.instance.winner = "Player";
        } else if(boardManagerPrefab.isGameOver(boardManagerPrefab.getBoard()) == GameManager.instance.aiMagic)
        {
            GameManager.instance.winner = "AI";
        } else
        {
            GameManager.instance.winner = "Draw";
        }
        StartCoroutine(callScene());
    }

    IEnumerator callScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }
}
