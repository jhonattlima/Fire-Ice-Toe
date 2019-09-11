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
            case "sceneBoard":
                SceneManager.LoadScene(GameManager.instance.sceneBoard, LoadSceneMode.Single);
                break;
            case "sceneGameOver":
                SceneManager.LoadScene(GameManager.instance.sceneGameOver, LoadSceneMode.Single);
                break;
            case "sceneMainMenu":
                SceneManager.LoadScene(GameManager.instance.sceneMainMenu, LoadSceneMode.Single);
                break;
            case "quit":
                Application.Quit();
                break;
        }
    }

}
