using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class OnlineDiscovery : MonoBehaviour
{
    //Variables
    private MatchInfoSnapshot[] storedMatches;
    private  float refreshTime = 3f;

    void Awake()
    {
        storedMatches = new MatchInfoSnapshot[Constants.SYSTEM_MAXMATCHES];
    }

    public void startListeningMatches(){
        StartCoroutine(routineListenMatches());
    }

    public void stopListeningMatches()
    {
        StopAllCoroutines();
    }

    public void createMatch(string name )
    {
        NetworkController.Match.CreateMatch(name, 2, true, "", "", "", 0, 0 , NetworkController.singleton.OnMatchCreate);
    }

    public void enterOnMatch(ButtonMatchController button)
    {
        NetworkController.Match.JoinMatch(button.match.networkId, "", "", "", 0, 0, NetworkController.singleton.OnMatchJoined);
    }

    private IEnumerator routineListenMatches()
    {
        List<MatchInfoSnapshot> updatedMatches;
        NetworkController.Match.ListMatches(0, GameManager.instance.maxMatches, "", true, 0, 0, NetworkController.singleton.OnMatchList);
        updatedMatches = NetworkController.singleton.matches;
        int counter = 0;
        if(updatedMatches != null && updatedMatches.Count > 0)
        {
            foreach(var match in updatedMatches)
            {
                if (counter >= GameManager.instance.maxMatches) break;
                updatedMatches[counter] = match;
                counter ++;
            }
            while(counter < GameManager.instance.maxMatches)
            {
                updatedMatches[counter] = null;
                counter ++;
            }
            Debug.Log("Reached end of listenMatches");
            UIManager.instance.updateMatchesList(storedMatches);
        }
        yield return new WaitForSeconds(refreshTime);
    }
}
