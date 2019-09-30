using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LanController : MonoBehaviour
{
    // Variables
    private string _matchName;
    private bool _isConnected;
    private bool _working = false;
    private NetworkBroadcastResult[] _currentMatchesData = new NetworkBroadcastResult[GameManager.instance.maxMatches];
    private Dictionary<NetworkBroadcastResult, float> activematches = new Dictionary<NetworkBroadcastResult, float>();
    private float refreshRate = 3f;
    private List<NetworkBroadcastResult> _tempMatchesData = new List<NetworkBroadcastResult>();

    public void startController(){
        // Start the discovery service
        NetworkController.Discovery.Initialize();
        // Discovery service start listening broadcast packets
        NetworkController.Discovery.StartAsClient();
        NetworkController.onServerConnect += someoneConnected;
        _working = true;
    }

    public void stopController(){
        NetworkController.Discovery.StopBroadcast();
        NetworkController.singleton.StopHost();
        _isConnected = false;
        _working = false;
    }

    // Variables used only during updates
    private float _updateCooldown = 0f;
    void Update()
    {
        // Refresh matches
        if(_working && !_isConnected){
            _updateCooldown -= Time.deltaTime;
            if(_updateCooldown <= 0){
                RefreshSessionsList();
                _updateCooldown = GameManager.instance.updateListTime;
            }
        }
    }

    private void someoneConnected(NetworkConnection conn){
        //start game
    }

    // Creates a match
    public void CreateMatch(string matchName){
        NetworkController.Discovery.StopBroadcast();
        NetworkController.Discovery.broadcastData = matchName;
        NetworkController.Discovery.StartAsServer();
        NetworkController.singleton.StartHost();
        _isConnected = true;
    }

    // Refresh Available matches list
    private void RefreshSessionsList(){
        int counter = 0;
        bool hasNew = false;
        foreach(var match in NetworkController.Discovery.broadcastsReceived.Values){
            if(counter >= GameManager.instance.maxMatches){
                break;
            } else {
                _currentMatchesData[counter] = match;
                if(!activematches.ContainsKey(match)){
                    activematches.Add(match, refreshRate + Time.time);
                    hasNew = true;
                } else {
                    activematches[match] = Time.time + refreshRate;
                }
                counter ++;
            }
            // clean obsolete values 
            var keys = activematches.Keys.ToList();
            if(keys != null){
                foreach(var key in keys){
                    if(activematches[key] <= Time.time){
                        activematches.Remove(key);
                        hasNew = true;
                    }
                }
            }
            NetworkController.Discovery.broadcastsReceived.Clear();
        }
        if(hasNew) UIManager.instance.updateMatchesList(activematches);
    }

    // Compare if two arrays are equals
    private bool isEqual(byte[] a, byte[] b){
        if(a.Length != b.Length) return false;
        for (int i = 0; i<a.Length; i++){
            if(a[i] != b[i]){
                return false;
            }
        }
        return true;
    }
}
