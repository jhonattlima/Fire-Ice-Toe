using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class OnlineDiscovery : MonoBehaviour
{
    //Variables
    private MatchInfoSnapshot[] storedMatches;
 
    void Awake()
    {
        storedMatches = new MatchInfoSnapshot[Constants.SYSTEM_MAXMATCHES];
    }

    public void startListeningMatches(){
        Debug.Log("Online Discovery says: Started to listen new matches.");
        //NetworkController.singleton.StartMatchMaker();
        StartCoroutine(routineListenMatches());
    }

    public void stopListeningMatches()
    {
        Debug.Log("Online Discovery says: Stopped to listen new matches.");
        StopAllCoroutines();
    }

    public void createMatch(string name )
    {
        Debug.Log("Online Discovery says: Created match. Waiting someone to connect.");
        NetworkController.Match.CreateMatch(name, 2, true, "", "", "", 0, 0 , NetworkController.singleton.OnMatchCreate);
        stopListeningMatches();
    }

    public void enterOnMatch(ButtonMatchController button)
    {
        Debug.Log("Online Discovery says: Entering on match.");
        stopListeningMatches();
        NetworkController.Match.JoinMatch(button.match.networkId, "", "", "", 0, 0, NetworkController.singleton.OnMatchJoined);
    }

    public void cancelOnlineDiscovery()
    {
        Debug.Log("Online Discovery says: Cancelling match.");
        OnlineOrquestrator.cancelOnlineMatch();
        // if(startedServer) NetworkController.Match.DestroyMatch(NetworkController.singleton.matchInfo.networkId, 
        // NetworkController.singleton.matchInfo.domain, OnMatchDestroy);
        // NetworkController.singleton.StopMatchMaker();
        // Destroy(OnlinePlayerController.localPlayer.gameObject);
        // if(startedServer) NetworkController.singleton.StopServer();
        // startedServer = false;
    }

    private IEnumerator routineListenMatches()
    {
        List<MatchInfoSnapshot> updatedMatches;
        Debug.Log("Online Discovery says: Listening matches.");
        while(true)
        {
            NetworkController.Match.ListMatches(0, Constants.SYSTEM_MAXMATCHES, "", true, 0, 0, NetworkController.singleton.OnMatchList);
            updatedMatches = NetworkController.singleton.matches;
            int counter = 0;
            if(updatedMatches != null && updatedMatches.Count > 0)
            {
                foreach(var match in updatedMatches)
                {
                    if (counter < Constants.SYSTEM_MAXMATCHES)
                    {
                        storedMatches[counter] = match;
                        counter ++; 
                    } break;
                }
            }
            while(counter < Constants.SYSTEM_MAXMATCHES)
            {
                    storedMatches[counter] = null;
                    counter ++;
            }
            UIManager.instance.updateMatchesList(storedMatches);
            yield return new WaitForSeconds(Constants.SYSTEM_ONLINE_MATCHES_REFRESHTIME);
        }
    }
}
