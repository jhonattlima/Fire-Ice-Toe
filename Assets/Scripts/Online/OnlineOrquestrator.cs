using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineOrquestrator
{
    public static int turn = 0; // Global reference for turn
    public static int player1Magic = 0; // Global reference for player 1 magic type

    public static void clear(){ // Clear this class to start again
        turn = 0;
        player1Magic = 0;
    }
}