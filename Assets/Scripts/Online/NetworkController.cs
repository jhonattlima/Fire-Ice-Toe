using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkController : NetworkManager
{
    // Variables
    public static event Action<NetworkConnection> onServerConnect;
    public static event Action<NetworkConnection> onClientConnect;

    // Return Network Discovery component
    public static NetworkDiscovery Discovery{
        get{
            return singleton.GetComponent<NetworkDiscovery>();
        }
    }

    // Return Match componente or create one if it does not exists
    public static NetworkMatch Match{
        get{
            return singleton.GetComponent<NetworkMatch>() ?? singleton.gameObject.AddComponent<NetworkMatch>();
        }
    }

    public override void OnServerConnect(NetworkConnection conn){
        base.OnServerConnect(conn);
        if(!conn.address.Equals("localClient")){
            onServerConnect?.Invoke(conn);
            Discovery.StopBroadcast();
        }
        //Debug.Log("Number of players: " + numPlayers);
    }

    public override void OnClientConnect(NetworkConnection conn){
        base.OnClientConnect(conn);
        if(!conn.address.Equals("localServer")){
            onClientConnect?.Invoke(conn);
        }
    }

    // No ideia why
    public override void OnClientError(NetworkConnection conn, int errorCode){
        base.OnClientError(conn, errorCode);
    }
}
