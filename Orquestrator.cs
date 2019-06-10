using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orquestrator : MonoBehaviour
{
    // Variables
    private bool _turn;
    [SerializeField]
    private BoardManager _boardManagerPrefab;
    [SerializeField]
    private AIController _aiPrefab;
    private GameManager _gameManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _gameManagerPrefab = GameObject.Find("GameManager").GetComponent<GameManager>();
        _turn = _gameManagerPrefab.getTurn();
        Debug.Log("Orquestrator: Player magic is: " + _gameManagerPrefab.getPlayerMagic());
        Debug.Log("Orquestrator: AI magic is: " + _gameManagerPrefab.getAIMagic());
        Debug.Log("Orquestrator: Turn is: " + _gameManagerPrefab.getTurn());
        Debug.Log("Orquestrator: BoardSize is: " + _gameManagerPrefab.getBoardSize());
        Debug.Log("Orquestrator: Difficulty is: " + _gameManagerPrefab.getDifficulty());
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
            if (_boardManagerPrefab.getHit())
            {
                _turn = false;
            }
        }
        else
        {
            if (_aiPrefab.play())
            {
                _turn = true;
            }
        }
        if (_boardManagerPrefab.isGameOver(_boardManagerPrefab.getBoard()) != 0)
        {
            GameOver();
        }
    }

    // Calls Game over screen
    private void GameOver()
    {
        if(_boardManagerPrefab.isGameOver(_boardManagerPrefab.getBoard()) == _gameManagerPrefab.getPlayerMagic())
        {
            _gameManagerPrefab.setWinner("Player");
        } else if(_boardManagerPrefab.isGameOver(_boardManagerPrefab.getBoard()) == _gameManagerPrefab.getAIMagic())
        {
            _gameManagerPrefab.setWinner("AI");
        } else
        {
            _gameManagerPrefab.setWinner("Draw");
        }
        StartCoroutine(callScene());
    }

    IEnumerator callScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }
}
