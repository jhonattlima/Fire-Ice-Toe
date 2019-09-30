using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class LanDiscovery : NetworkDiscovery
{
//     private float refreshTime = 3f;
//     private Dictionary<ConnInfo, float> connectionInfos = new Dictionary<ConnInfo, float>();

//     // Start is called before the first frame updat

//     private void Awake(){
//         StartCoroutine(cleanOldSRegisters());
//     }

//     public void listenMatches(){
//         base.Initialize();
//         base.StartAsClient();
//     }

//     public override void OnReceivedBroadcast(string fromAddress, string data)
//     {
//         base.OnReceivedBroadcast(fromAddress, data);
//         ConnInfo info = new ConnInfo(fromAddress, data);
//         if(!connectionInfos.ContainsKey(info)){
//             connectionInfos.Add(info, Time.time + refreshTime);
//             // Send data to UI
//         } else {
//             connectionInfos[info] = Time.time + refreshTime;
//         }
//     }

//     public void createMatch(string name){
//         StopBroadcast();
//         base.Initialize();
//         base.broadcastData = name;
//         base.StartAsServer();
//     }

//     private IEnumerator cleanOldSRegisters(){
//         while(true)
//         {
//             Debug.Log("Entered here");
//             bool changed = false;
//             var keys = connectionInfos.Keys.ToList();
//             if(keys != null){
//                 foreach(var key in keys){
//                     if(connectionInfos[key] <= Time.time){
//                         connectionInfos.Remove(key);
//                         changed = true;
//                     }
//                 }
//             }
//             if(changed) UIManager.instance.updateMatchesList(connectionInfos);
//             yield return new WaitForSeconds(refreshTime);
//         }
//     }
}

public struct ConnInfo{
    public string address;
    public string name;
    public ConnInfo(string fromAddress, string data){
        this.address = fromAddress;
        name = data;
    }
}
