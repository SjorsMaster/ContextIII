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
    [SerializeField] private Transform global;
    [SerializeField] private GameObject anchorPrefab;

    public readonly SyncDictionary<string, string> ReferenceWorld = new(); // Key: anchor uuid, Value: world name.

    public bool IsBusy => isBusy;

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
        else if (msg.TargetWorld == "Global")
        {
            if (ReferenceWorld.TryAdd(msg.AnchorUUID, msg.TargetWorld))
            {
                manager.ReferenceAnchors[msg.AnchorUUID].SpatialAnchor.gameObject.transform.SetParent(global);
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
        if (!World.worlds.ContainsKey(worldName) && worldName != "Global")
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
            VRDebugPanel.Instance.SendDebugMessage($"Adding user {id} to the anchor sharing list...");
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
                VRDebugPanel.Instance.SendDebugMessage("Anchor was unable to parent to the world on the server in time!");
                Destroy(spatialAnchor.gameObject);
                return;
            }

            await Task.Yield();
        }

        if (targetWorld == "Global")
        {
            spatialAnchor.gameObject.transform.SetParent(global);
            VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to Global!");
            return;
        }

        World world = World.worlds[targetWorld];
        spatialAnchor.gameObject.transform.SetParent(world.transform);
        world.Add(spatialAnchor.gameObject, false);

        VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to the world!");
    }

    public void ReparentAnchors()
    {
        VRDebugPanel.Instance.SendDebugMessage("Reparenting anchors...");
        foreach (var pair in ReferenceWorld)
        {
            string worldName = pair.Value;
            string uuid = pair.Key;

            if (worldName == "Global")
            {
                SSA.Anchors[Guid.Parse(uuid)].gameObject.transform.SetParent(global);
                VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to Global!");
            }
            else if (World.worlds.TryGetValue(worldName, out World world))
            {
                SSA.Anchors[Guid.Parse(uuid)].gameObject.transform.SetParent(world.transform);
                world.Add(SSA.Anchors[Guid.Parse(uuid)].gameObject, false);
                VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to the world!");
            }
        }

        foreach (var obj in FindObjectsOfType<DynamicWorldAnchoredObject>())
        {
            obj.CorrectToWorld();
        }
    }
}

public struct MsgParentToWorld : NetworkMessage
{
    public string TargetWorld;
    public string AnchorUUID;
}
