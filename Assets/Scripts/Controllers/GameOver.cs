using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image _gameOverImage;
    public Sprite _draw;
    public Sprite _win;
    public Sprite _lose;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Game manager winner: " + GameManager.instance.winner);
        if (GameManager.instance.winner.Equals("Draw")){
            MusicPlayer.instance.Play(GameManager.instance.musicDraw);
            _gameOverImage.sprite = _draw;
            //Debug.Log(" This game finished in draw! :(");
        } else if (GameManager.instance.winner.Equals("Player")) {
            MusicPlayer.instance.Play(GameManager.instance.musicWinner);
            _gameOverImage.sprite = _win;
            //Debug.Log("Player 1 wins the game!");
        } else
        {
            MusicPlayer.instance.Play(GameManager.instance.musicLoser);
            _gameOverImage.sprite = _lose;
            //Debug.Log("AI wins the game!");
        }
        StartCoroutine(restartGame());
    }

    IEnumerator restartGame()
    {
        NetworkController.singleton.StopHost();
        string sceneMainMenu = GameManager.instance.sceneMainMenu; 
        Destroy(GameManager.instance.gameObject);
        Destroy(NetworkController.singleton.gameObject);
        OnlineOrquestrator.clear();
        yield return new WaitForSeconds(5);
        SceneController.instance.changeScene(sceneMainMenu);
    }
}
