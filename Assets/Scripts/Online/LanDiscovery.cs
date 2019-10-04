using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

// Class used to organize broadcast data
public class StoredData{
    public string fromAddress;
    public string id;
    public string data;
    public float expiration;
    public StoredData(string fromAddress, string id, string data, float expiration){
        this.fromAddress = fromAddress;
        this.id = id;
        this.data = data;
        this.expiration = expiration;
    }
}

public class LanDiscovery : NetworkDiscovery
{
    // Variables
    private float refreshTime = 1f;
    private StoredData[] storedDatas;

    void Start(){
        storedDatas = new StoredData[GameManager.instance.maxMatches];
        StartCoroutine(cleanOldSRegisters());
    }

    // Start listening matches
    public void listenMatches(){
        base.Initialize();
        base.StartAsClient();
    }

    // Create a new match
    public void createMatch(string name){
        StopBroadcast();
        base.Initialize();
        base.broadcastData = "fireicetoe/" + name + "/" + Random.Range(1, 10000);
        base.StartAsServer();
        Debug.Log("Lan Discovery says: Starting sending broadcast " + broadcastData);
    }

    // Handle the received matches
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Lan Discovery says: Received a new broadcast: " + data);
        base.OnReceivedBroadcast(fromAddress, data);
        bool changed = false;
        string[] splitData = data.Split('/');
        if(splitData[0].Equals("fireicetoe")){
            StoredData info = new StoredData(fromAddress, splitData[splitData.Length-1],  splitData[1], Time.time + refreshTime);
            
            for(int i =0; i < GameManager.instance.maxMatches; i++){
                if(storedDatas[i] != null && storedDatas[i].id.Equals(info.id)){
                    storedDatas[i].expiration = Time.time+refreshTime;
                    break;
                }
                else if(storedDatas[i] == null){
                    storedDatas[i] = info;
                    changed = true;
                    break;
                }
            }
            if(changed) UIManager.instance.updateMatchesList(storedDatas);
        }
    }

    // Loop routine to clean old rooms that might not be available anymore
    private IEnumerator cleanOldSRegisters(){
        while(true)
        {
            bool changed = false;
            for (int i = 0; i<GameManager.instance.maxMatches; i++){
                if(storedDatas[i] != null && storedDatas[i].expiration <= Time.time){
                    storedDatas[i] = null;
                    changed = true;
                }
            }
            if(changed) UIManager.instance.updateMatchesList(storedDatas);
            yield return new WaitForSeconds(refreshTime);
        }
    }
}