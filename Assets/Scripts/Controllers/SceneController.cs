using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public void changeScene(string sceneName)
    {
        switch (sceneName)
        {
            case Constants.SCENE_BOARD:
                SceneManager.LoadScene(GameManager.instance.sceneBoard, LoadSceneMode.Single);
                MusicPlayer.instance.Play(GameManager.instance.musicBoard);
                break;
            case Constants.SCENE_MENU:
                SceneManager.LoadScene(GameManager.instance.sceneMainMenu, LoadSceneMode.Single);
                MusicPlayer.instance.Play(GameManager.instance.musicMainMenu);
                break;
            case Constants.SCENE_GAMEOVER:
                SceneManager.LoadScene(GameManager.instance.sceneGameOver, LoadSceneMode.Single);
                break;
            case Constants.SYSTEM_QUITGAME:
                Application.Quit();
                break;
        }
    }
}
