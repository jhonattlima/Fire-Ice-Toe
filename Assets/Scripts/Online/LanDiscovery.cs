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
    private StoredData[] storedDatas;
    private bool isBroadcasting = false;
    private bool isHost = false;

    public void listenMatches()
    {   // Start listening matches
        storedDatas = null;
        storedDatas = new StoredData[Constants.SYSTEM_MAXMATCHES];
        StartCoroutine(cleanOldSRegisters());
        base.Initialize();
        base.StartAsClient();
        isBroadcasting = true;
        Debug.Log("Lan Discovery says: Started to listen to matches.");
    }

    public void createMatch(string name)
    {   // Create a new match
        StopBroadcast();
        base.broadcastData = "fireicetoe/" + name + "/" + Random.Range(1, 10000);
        StartAsServer();
        NetworkController.singleton.StartHost();
        isHost = true;
        Debug.Log("Lan Discovery says: Created match " + broadcastData + ". Waiting someone to connect.");
    }

    public void enterOnMatch(ButtonMatchController button)
    {   // Enter on the game
        StopAllCoroutines();
        if(isBroadcasting) StopBroadcast();
        isBroadcasting = false;
        NetworkController.singleton.networkAddress = button.lanMatch.fromAddress;
        NetworkController.singleton.StartClient();
        Debug.Log("Lan Discovery says: Entered on a match.");
    }

    // Cancel everything;
    public void cancelLanDiscovery(){
        StopAllCoroutines();
        if(isBroadcasting) StopBroadcast();
        if(isHost)
        { 
            NetworkController.singleton.StopHost();
        }
        isHost= false;
        isBroadcasting = false;
    }

    // Handle the received matches
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Lan Discovery says: Received a new broadcast: " + data);
        base.OnReceivedBroadcast(fromAddress, data);
        if(!fromAddress.Contains("10.")) return; // Turn on on PUCRS
        bool changed = false;
        string[] splitData = data.Split('/');
        if(splitData[0].Equals("fireicetoe"))
        {
            StoredData info = new StoredData(fromAddress, splitData[splitData.Length-1],  splitData[1], Time.time + Constants.SYSTEM_ONLINE_MATCHES_REFRESHTIME);
            for(int i =0; i < Constants.SYSTEM_MAXMATCHES; i++)
            {
                if(storedDatas[i] != null && storedDatas[i].id.Equals(info.id))
                {
                    storedDatas[i].expiration = Time.time + Constants.SYSTEM_ONLINE_MATCHES_REFRESHTIME;
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
            for (int i = 0; i < Constants.SYSTEM_MAXMATCHES; i++)
            {
                if(storedDatas[i] != null && storedDatas[i].expiration <= Time.time){
                    storedDatas[i] = null;
                    changed = true;
                }
            }
            if(changed) UIManager.instance.updateMatchesList(storedDatas);
            yield return new WaitForSeconds(Constants.SYSTEM_ONLINE_MATCHES_REFRESHTIME);
        }
    }
}
