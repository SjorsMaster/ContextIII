using Mirror;
using SharedSpaces;
using SharedSpaces.Managers;
using SharedSpaces.NetorkMessages;
using SharedSpaces.Singletons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldsAnchorManager : NetworkSingleton<WorldsAnchorManager>
{
    [SerializeField] private GameObject anchorPrefab;

    public readonly SyncDictionary<string, string> ReferenceWorld = new(); // Key: anchor uuid, Value: world name.

    private bool isBusy;

    #region Event Handlers
    private void SSA_OnDebugMessage(string msg)
    {
        VRDebugPanel.Instance.SendDebugMessage(msg);
    }

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

    private void OnEnable()
    {
        SSA.OnDebugMessage += SSA_OnDebugMessage;
    }

    private void OnDisable()
    {
        SSA.OnDebugMessage -= SSA_OnDebugMessage;
    }

    public async void PlaceAnchorInWorld(string worldName)
    {
        if (!World.worlds.ContainsKey(worldName))
        {
            throw new Exception("World not found!");
        }

        if (isBusy)
        {
            return;
        }

        isBusy = true;

        try
        {
            Transform target = TrackedObjectsManager.Instance.RightHandAnchor;
            void handler(SpatialAnchor anchor) => SSA_OnAnchorCreated(anchor, worldName);
            SSA.OnAnchorCreated += handler;
            await SSA.CreateSpatialAnchor(anchorPrefab, target.position, target.rotation);
            SSA.OnAnchorCreated -= handler;
        }
        catch (Exception e)
        {
            VRDebugPanel.Instance.SendDebugMessage($"Failed to place an anchor! {e}");
        }

        isBusy = false;
    }

    private async void SSA_OnAnchorCreated(SpatialAnchor spatialAnchor, string targetWorld)
    {
        ServerManager serverManager = ServerManager.TryGetInstance();
        if (serverManager == null)
        {
            VRDebugPanel.Instance.SendDebugMessage("Server manager not found!");
            return;
        }

        VRDebugPanel.Instance.SendDebugMessage("Sharing anchor...");
        List<OVRSpaceUser> spaceUsers = new();

        foreach (var id in serverManager.JoinedOculusIDs)
        {
            spaceUsers.Add(new OVRSpaceUser(id));
        }

        await SSA.ShareAnchors(spaceUsers, new SpatialAnchor[1] { spatialAnchor });

        string uuid = spatialAnchor.Anchor.Uuid.ToString();
        MsgAddAnchorData addAnchorData = new()
        {
            Data = new()
            {
                Position = spatialAnchor.transform.position,
                Rotation = spatialAnchor.transform.rotation,
                UUID = uuid,
            }
        };
        NetworkClient.Send(addAnchorData);

        float timeout = 10f;
        float time = Time.time;
        while (!serverManager.ReferenceAnchors.ContainsKey(uuid))
        {
            if (Time.time - time > timeout)
            {
                VRDebugPanel.Instance.SendDebugMessage("Anchor did not reach server before timeout!");
                Destroy(spatialAnchor.gameObject);
                return;
            }

            await Task.Yield();
        }

        NetworkClient.Send(new MsgParentToWorld()
        {
            TargetWorld = targetWorld,
            AnchorUUID = uuid,
        });

        time = Time.time;
        while (!ReferenceWorld.ContainsKey(uuid))
        {
            if (Time.time - time > timeout)
            {
                VRDebugPanel.Instance.SendDebugMessage("Anchor was unable to the world on the server in time!");
                Destroy(spatialAnchor.gameObject);
                return;
            }

            await Task.Yield();
        }

        World world = World.worlds[targetWorld];
        spatialAnchor.gameObject.transform.SetParent(world.transform);
        world.Add(spatialAnchor.gameObject, false);

        VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to the world!");
    }
}

public struct MsgParentToWorld : NetworkMessage
{
    public string TargetWorld;
    public string AnchorUUID;
}
