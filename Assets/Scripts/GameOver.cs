using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private GameManager _gameManagerPrefab;
    [SerializeField]
    private Image _gameOverImage;
    [SerializeField]
    private AudioClip _musicPlayerWin;
    [SerializeField]
    private AudioClip _musicAIWin;
    [SerializeField]
    private AudioClip _musicDraw;
    [SerializeField]
    private Sprite _draw;
    [SerializeField]
    private Sprite _win;
    [SerializeField]
    private Sprite _lose;

    // Start is called before the first frame update
    void Start()
    {
        _gameManagerPrefab = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Game manager winner: " + GameManager.instance.winner);
        if (GameManager.instance.winner.Equals("Draw")){
            AudioSource.PlayClipAtPoint(_musicDraw, transform.position);
            _gameOverImage.sprite = _draw;
            Debug.Log(" This game finished in draw! :(");
        } else if (GameManager.instance.winner.Equals("Player")) {
            AudioSource.PlayClipAtPoint(_musicPlayerWin, transform.position);
            _gameOverImage.sprite = _win;
            Debug.Log("Player 1 wins the game!");
        } else
        {
            MusicPlayer.instance.Play(GameManager.instance.musicLoser);
            _gameOverImage.sprite = _lose;
            Debug.Log("AI wins the game!");
        }
        StartCoroutine(restartGame());
    }

    IEnumerator restartGame()
    {
        string sceneMainMenu = GameManager.instance.sceneMainMenu; 
        Destroy(_gameManagerPrefab.gameObject);
        yield return new WaitForSeconds(5);
        SceneController.instance.changeScene(sceneMainMenu);
    }
}
