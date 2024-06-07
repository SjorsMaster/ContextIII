using Mirror;
using SharedSpaces.Managers;
using SharedSpaces.Singletons;
using System.Collections.Generic;

public class WorldMsgHandler : NetworkPersistentSingleton<WorldMsgHandler>
{
    public readonly SyncDictionary<string, string> ReferenceWorld = new(); // Key: anchor uuid, Value: world name.

    #region Event Handlers
    private void OnParentToWorld(NetworkConnectionToClient sender, MsgParentToWorld msg)
    {
        ServerManager manager = ServerManager.TryGetInstance();
        if (manager == null)
        {
            return;
        }

        if (World.worlds.TryGetValue(msg.TargetWorld, out World world))
        {
            if (ReferenceWorld.TryAdd(msg.AnchorUUID, msg.TargetWorld))
            {
                manager.ReferenceAnchors[msg.AnchorUUID].SpatialAnchor.gameObject.transform.SetParent(world.transform);
            }
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<MsgParentToWorld>(OnParentToWorld);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        NetworkServer.UnregisterHandler<MsgParentToWorld>();
    }
}
