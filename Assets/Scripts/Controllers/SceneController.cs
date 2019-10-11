using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void changeScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Board 3x3":
                SceneManager.LoadScene(GameManager.instance.sceneBoard, LoadSceneMode.Single);
                MusicPlayer.instance.Play(GameManager.instance.musicBoard);
                break;
            case "Menu":
                SceneManager.LoadScene(GameManager.instance.sceneMainMenu, LoadSceneMode.Single);
                MusicPlayer.instance.Play(GameManager.instance.musicMainMenu);
                break;
            case "Game Over":
                SceneManager.LoadScene(GameManager.instance.sceneGameOver, LoadSceneMode.Single);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
