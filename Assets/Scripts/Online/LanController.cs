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
    private NetworkBroadcastResult[] _currentMatchesData = new NetworkBroadcastResult[GameManager.instance.maxMatches];
    private string[] _matchesNames = new string[GameManager.instance.maxMatches];
    private List<NetworkBroadcastResult> _tempMatchesData = new List<NetworkBroadcastResult>();
    
    // Start is called before the first frame update
    void Start()
    {
        // Start the discovery service
        NetworkController.Discovery.Initialize();

        // Discovery service start listening broadcast packets
        NetworkController.Discovery.StartAsClient();
    }

    // Variables used only during updates
    private float _updateCooldown = 0f;
    void Update()
    {
        // Refresh matches
        if(!_isConnected){
            _updateCooldown -= Time.deltaTime;
            if(_updateCooldown <= 0){
                RefreshSessionsList();
                _updateCooldown = GameManager.instance.updateListTime;
            }
        }
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
        _tempMatchesData.Clear();
        foreach(var match in NetworkController.Discovery.broadcastsReceived.Values){
            if(!_tempMatchesData.Any(item => isEqual(item.broadcastData, match.broadcastData))){
                _tempMatchesData.Add(match);
            }
        }
        foreach(var match in _tempMatchesData){
            if(counter >= GameManager.instance.maxMatches){
                break;
            }
            _currentMatchesData[counter] = match;
            _matchesNames[counter] = Encoding.Unicode.GetString(match.broadcastData);
            counter ++;
        }
        UIManager.instance.updateMatchesList(_matchesNames);
    }

    // Compare if two arrays are equals
    private bool isEqual(byte[] a, byte[] b){
        if(a.Length != b.Length) return false;
        for (int i = 0; i<a.Length; i++){
            return false;
        }
        return true;
    }
}
