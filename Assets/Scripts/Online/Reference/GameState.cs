using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{

    private static int[] _stateValues = new int[5];

    public static event Action OnStateUpdated;

    private static List<PlayerController> _players = new List<PlayerController>();

    // Roda apenas no servidor
    public static void RegisterPlayer(PlayerController player)
    {
        _players.Add(player);
    }

    public static void UpdateValue(int pos, int val, bool isServer)
    {
        Debug.Log($"UpdateValue called! pos:{pos} val:{val} isServer:{isServer}");

        _stateValues[pos] = val;
        OnStateUpdated?.Invoke();

        if (isServer)
        {
            foreach (var player in _players)
            {
                player.UpdateValue(pos, val);
            }
        }
    }

    public static int GetValue(int pos)
    {
        return _stateValues[pos];
    }

}
