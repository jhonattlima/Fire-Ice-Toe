using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class OnlineDiscovery : MonoBehaviour
{
    //Variables
    private MatchInfoSnapshot[] storedMatches;
    private  float refreshTime = 1f;

    void Awake()
    {
        storedMatches = new MatchInfoSnapshot[Constants.SYSTEM_MAXMATCHES];
    }

    public void startListeningMatches(){
        Debug.Log("Online Discovery says: Started to listen new matches.");
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
        stopListeningMatches();
        Debug.Log("Online Discovery says: Entering on match.");
        NetworkController.Match.JoinMatch(button.match.networkId, "", "", "", 0, 0, NetworkController.singleton.OnMatchJoined);
    }

    public void cancelMatch()
    {

    }

    private IEnumerator routineListenMatches()
    {
        List<MatchInfoSnapshot> updatedMatches;
        while(true)
        {
            NetworkController.Match.ListMatches(0, GameManager.instance.maxMatches, "", true, 0, 0, NetworkController.singleton.OnMatchList);
            updatedMatches = NetworkController.singleton.matches;
            int counter = 0;
            if(updatedMatches != null && updatedMatches.Count > 0)
            {
                foreach(var match in updatedMatches)
                {
                    if (counter >= GameManager.instance.maxMatches) break;
                    storedMatches[counter] = match;
                    counter ++;
                }
                while(counter < GameManager.instance.maxMatches)
                {
                    storedMatches[counter] = null;
                    counter ++;
                }
                Debug.Log("Reached end of listenMatches");
                UIManager.instance.updateMatchesList(storedMatches);
            }
            yield return new WaitForSeconds(refreshTime);
        }
    }
}
