using Mirror;
using SharedSpaces;
using SharedSpaces.Managers;
using SharedSpaces.NetorkMessages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldsAnchorManager : MonoBehaviour
{
    [SerializeField] private GameObject anchorPrefab;

    private bool isBusy;

    #region Event Handlers
    private void SSA_OnDebugMessage(string obj)
    {
        VRDebugPanel.Instance.SendDebugMessage(obj);
    }
    #endregion

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
        WorldMsgHandler worldMsgHandler = WorldMsgHandler.TryGetInstance();
        if (serverManager == null || worldMsgHandler == null)
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
        while (!worldMsgHandler.ReferenceWorld.ContainsKey(uuid))
        {
            if (Time.time - time > timeout)
            {
                VRDebugPanel.Instance.SendDebugMessage("Anchor was unable to the world on the server in time!");
                Destroy(spatialAnchor.gameObject);
                return;
            }

            await Task.Yield();
        }

        spatialAnchor.gameObject.transform.SetParent(World.worlds[targetWorld].transform);

        VRDebugPanel.Instance.SendDebugMessage("Anchor was successfully parented to the world!");
    }
}

public struct MsgParentToWorld : NetworkMessage
{
    public string TargetWorld;
    public string AnchorUUID;
}
