using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkController : NetworkManager
{
    // Variables
    public static event Action<NetworkConnection> onlineConection; // For online connections
    public static event Action<NetworkConnection> lanConnection; // For Lan connections

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

    // Check if the connection type is online and address it to onlineConnection var if it is
    public override void OnServerConnect(NetworkConnection conn){
        base.OnServerConnect(conn);
        if(!conn.address.Equals("localClient")){
            Debug.Log("Client has connected to Server! " +conn.address);
            onlineConection?.Invoke(conn);
        }
    }

    // Check if the connection type is lan and address it to lanConnection var if it is
    public override void OnClientConnect(NetworkConnection conn){
        base.OnClientConnect(conn);
        if(!conn.address.Equals("localServer")){
            Debug.Log("Client has connected to Lan! " +conn.address);
            lanConnection?.Invoke(conn);
        }
    }

    // No ideia why
    public override void OnClientError(NetworkConnection conn, int errorCode){
        base.OnClientError(conn, errorCode);
    }
}
