using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    public static GameManager instance;
    public int playerMagic; // 1 = fire, 2= ice
    public int aiMagic; // !playerMagic
    public string difficulty; // Easy, Impossible
    public int boardSize = 3; // 3
    public bool initialTurn; // true = player, false = AI
    public string winner; // Player 1, AI or Draw
    public string musicBoard = "musicBoard";
    public string musicMainMenu = "musicMainMenu";
    public string musicDraw = "musicDraw";
    public string musicWinner = "musicWinner";
    public string musicLoser = "musicLoser";
    public string SFXFire = "sfxFire";
    public string SFXIce = "sfxIce";
    public string sceneBoard = "Board 3x3";
    public string sceneMainMenu = "Menu";
    public string sceneGameOver = "Game Over";
    public bool turn;

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

    public void setPlayerMagic(int playerMagic)
    {
        this.playerMagic = playerMagic;
        if (playerMagic == 1)
        {
            aiMagic = 2;
        }
        else
        {
            aiMagic = 1;
        }
    }
}
