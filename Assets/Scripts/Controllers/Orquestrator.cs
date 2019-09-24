using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Orquestrator : MonoBehaviour
{
    // Variables
    public static Orquestrator instance;
    private bool _turn;
    public AIController aiPrefab;

    void Awake(){
                if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _turn = GameManager.instance.turn;
        //Debug.Log("Orquestrator: Player magic is: " + GameManager.instance.playerMagic);
        //Debug.Log("Orquestrator: AI magic is: " + GameManager.instance.aiMagic);
        //Debug.Log("Orquestrator: Turn is: " + GameManager.instance.turn);
        //Debug.Log("Orquestrator: BoardSize is: " + GameManager.instance.boardSize);
        //Debug.Log("Orquestrator: Difficulty is: " + GameManager.instance.difficulty);
    }

    // Update is called once per frame - WORKING
    void Update()
    {
        if(GameManager.instance.mode.Equals("offline")){
            checkTurn();
        }
    }

    private void checkTurn()
    {
        if (_turn)
        { // Player turn = true
            if (BoardManager.instance.getHit())
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
        if (BoardManager.instance.isGameOver(BoardManager.instance.getBoard()) != 0)
        {
            GameOver();
        }
    }

    // Calls Game over screen
    private void GameOver()
    {
        if(BoardManager.instance.isGameOver(BoardManager.instance.getBoard()) == GameManager.instance.playerMagic)
        {
            GameManager.instance.winner = "Player";
        } else if(BoardManager.instance.isGameOver(BoardManager.instance.getBoard()) == GameManager.instance.aiMagic)
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
        SceneController.instance.changeScene(GameManager.instance.sceneGameOver);
    }
}
