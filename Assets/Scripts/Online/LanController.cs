using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LanController : MonoBehaviour
{
    // // Variables
    // private string _matchName;
    // private bool _isConnected;
    // private bool _working = false;
    // private float refreshRate = 5f;
    // private List<StoredData> _storedDatas = new List<StoredData>();

    // public void startController(){
    //     // Start the discovery service
    //     NetworkController.Discovery.Initialize();
    //     // Discovery service start listening broadcast packets
    //     NetworkController.Discovery.StartAsClient();
    //     NetworkController.onServerConnect += someoneConnected;
    //     _working = true;
    // }

    // public void stopController(){
    //     NetworkController.Discovery.StopBroadcast();
    //     NetworkController.singleton.StopHost();
    //     _isConnected = false;
    //     _working = false;
    // }

    // // Variables used only during updates
    // private float _updateCooldown = 0f;
    // void Update()
    // {
    //     // Refresh matches
    //     if(_working && !_isConnected){
    //         _updateCooldown -= Time.deltaTime;
    //         if(_updateCooldown <= 0){
    //             RefreshSessionsList();
    //             _updateCooldown = GameManager.instance.updateListTime;
    //         }
    //     }
    // }

    // private void someoneConnected(NetworkConnection conn){
    //     //start game
    // }

    // // Creates a match
    // public void CreateMatch(string matchName){
    //     NetworkController.Discovery.StopBroadcast();
    //     NetworkController.Discovery.broadcastData = matchName + "/" + Random.Range(0, 10000);
    //     NetworkController.Discovery.StartAsServer();
    //     NetworkController.singleton.StartHost();
    //     _isConnected = true;
    // }

    // // Refresh Available matches list
    // private void RefreshSessionsList(){
    //     bool hasNew = false;
    //     //NetworkController.Discovery.broadcastsReceived?.Clear();
    //     foreach(var match in NetworkController.Discovery.broadcastsReceived.Values){
    //         // Check if the match is already in matcheslist
    //         bool isInList = false;
    //         string[] data =  Encoding.Unicode.GetString(match.broadcastData).Split('/');
    //         string id = data[data.Length-1];
    //         foreach(var item in _storedDatas){
    //             if(item.id.Equals(id)){
    //                 item.expiration = Time.time + refreshRate;
    //                 isInList = true;
    //             }
    //         }
    //         // It's a new match
    //         if(!isInList){
    //             _storedDatas.Add(new StoredData(id, data[0], Time.time + refreshRate));
    //             hasNew = true;
    //         }
    //     }
    //      // clean obsolete values 
    //     foreach(StoredData data in _storedDatas){
    //         if(data.expiration < Time.time){
    //             _storedDatas.Remove(data);
    //             hasNew = true;
    //         }
    //     }
    //     if(hasNew) UIManager.instance.updateMatchesList(_storedDatas);
    // }

    // // Compare if two arrays are equals
    // private bool isEqual(byte[] a, byte[] b){
    //     if(a.Length != b.Length) return false;
    //     for (int i = 0; i<a.Length; i++){
    //         if(a[i] != b[i]){
    //             return false;
    //         }
    //     }
    //     return true;
    // }
}
