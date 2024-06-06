using Mirror;
using UnityEngine;

public class MiniGamePlayerReference : NetworkBehaviour
{
    public MiniGamePlayer Player { get; private set; }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Player = NetworkClient.spawned[NetworkClient.localPlayer.netId].GetComponent<MiniGamePlayer>();

        CmdAddReference(NetworkClient.localPlayer.netId);
    }

    [Command]
    private void CmdAddReference(uint netId)
    {
        Player = NetworkServer.spawned[netId].GetComponent<MiniGamePlayer>();

        RpcAddReference(netId);
    }

    [ClientRpc]
    private void RpcAddReference(uint netId)
    {
        if (NetworkClient.localPlayer.netId == netId)
        {
            return;
        }

        Player = NetworkClient.spawned[netId].GetComponent<MiniGamePlayer>();
    }
}
