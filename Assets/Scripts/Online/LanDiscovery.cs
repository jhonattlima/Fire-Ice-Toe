using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class StoredData {
    public string id;
    public string data;
    public float expiration;
    public StoredData(string id, string data, float expiration){
        this.id = id;
        this.data = data;
        this.expiration = expiration;
    }
}

public class LanDiscovery : NetworkDiscovery
{
    private float refreshTime = 1f;
    private Dictionary<ConnInfo, float> connectionInfos = new Dictionary<ConnInfo, float>();

    // Start is called before the first frame updat
    private void Awake()
    {
        StartCoroutine(cleanOldSRegisters());
    }

    public void listenMatches(){
        base.Initialize();
        base.StartAsClient();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        bool changed = false;
        Debug.Log("received a new broadcast");
        base.OnReceivedBroadcast(fromAddress, data);
        string[] splitData = data.Split('/');
        if(data[0].Equals("fireicetoe")){
            ConnInfo info = new ConnInfo(fromAddress, splitData[1], splitData[splitData.Length-1]);
            if(!connectionInfos.ContainsKey(info)){
                Debug.Log("enteredHere");
                connectionInfos.Add(info, Time.time + refreshTime);
                changed = true;
            } else {
                connectionInfos[info] = Time.time + refreshTime;
            }
            if(changed) UIManager.instance.updateMatchesList(connectionInfos);
        }
    }

    public void createMatch(string name){
        StopBroadcast();
        base.Initialize();
        base.broadcastData = "fireicetoe/" + name + "/" + Random.Range(1, 10000);
        base.StartAsServer();
    }

    private IEnumerator cleanOldSRegisters(){
        while(true)
        {
            Debug.Log("Entered here");
            bool changed = false;
            var keys = connectionInfos.Keys.ToList();
            if(keys != null){
                foreach(var key in keys){
                    if(connectionInfos[key] <= Time.time){
                        connectionInfos.Remove(key);
                        changed = true;
                    }
                }
            }
            if(changed) UIManager.instance.updateMatchesList(connectionInfos);
            yield return new WaitForSeconds(refreshTime);
        }
    }
}

public struct ConnInfo{
    public string address;
    public string name;
    public string id;
    public ConnInfo(string fromAddress, string name, string id){
        this.address = fromAddress;
        this.name = name;
        this.id = id;
    }
}
