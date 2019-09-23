using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    
    private void Start()
    {
        if (isServer)
        {
            GameState.RegisterPlayer(this);
        }
    }

    // Chamado pelo cliente, executa no servidor
    [Command]
    private void CmdDoMove(int pos, int val)
    {
        GameState.UpdateValue(pos, val, true);
    }

    // Chamado pelo servidor, executa no cliente
    [ClientRpc]
    private void RpcUpdateValue(int pos, int val)
    {
        // garante que só é chamado no player que é dono do objeto
        if (!isLocalPlayer) return;

        GameState.UpdateValue(pos, val, false);
    }

    // Roda apenas no servidor. Replica atualização para os clientes.
    public void UpdateValue(int pos, int val)
    {
        RpcUpdateValue(pos, val);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            int movePos = Random.Range(0, 5);
            int moveVal = Random.Range(0, 100);
            CmdDoMove(movePos, moveVal);
        }
    }
}
