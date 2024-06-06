using Mirror;
using UnityEngine;

public class MiniGamePlayerReference : NetworkBehaviour
{
    [SerializeField] private MiniGamePlayer player;

    public MiniGamePlayer Player => player;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        if (isServer)
        {
            return;
        }

        player = NetworkClient.spawned[NetworkClient.localPlayer.netId].GetComponent<MiniGamePlayer>();

        CmdAddReference(NetworkClient.localPlayer.netId);
    }

    [Command]
    private void CmdAddReference(uint netId)
    {
        player = NetworkServer.spawned[netId].GetComponent<MiniGamePlayer>();

        RpcAddReference(netId);
    }

    [ClientRpc]
    private void RpcAddReference(uint netId)
    {
        if (NetworkClient.localPlayer.netId == netId)
        {
            return;
        }

        player = NetworkClient.spawned[netId].GetComponent<MiniGamePlayer>();
    }
}
