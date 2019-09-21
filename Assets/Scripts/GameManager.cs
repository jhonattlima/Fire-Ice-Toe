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
    public readonly string musicBoard = "musicBoard";
    public readonly string musicMainMenu = "musicMainMenu";
    public readonly string musicDraw = "musicDraw";
    public readonly string musicWinner = "musicWinner";
    public readonly string musicLoser = "musicLoser";
    public readonly string SFXFire = "sfxFire";
    public readonly string SFXIce = "sfxIce";
    public readonly string sceneBoard = "Board 3x3";
    public readonly string sceneMainMenu = "Menu";
    public readonly string sceneGameOver = "Game Over";
    public readonly string quitGame = "Quit";
    public readonly string buttonConfirmation = "buttonConfirmation";
    public readonly string buttonHightlight = "buttonHighlight";
    public readonly string charBurn = "burn";
    public readonly string charFreeze = "freeze";

    //public string diffEasy = "Easy";
    //public string diffHard = "Impossible";
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
