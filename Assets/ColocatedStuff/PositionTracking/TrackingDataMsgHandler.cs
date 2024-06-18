using Mirror;
using SharedSpaces;
using SharedSpaces.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDataMsgHandler : NetworkBehaviour
{
    [SerializeField] private TrackingSaveDataManager manager;
    [SerializeField] private TrackingSaveDataHandler handler;

    private readonly Dictionary<uint, RenderPath> activeRenders = new();

    #region Event Handlers
    private void OnTrackingDataReceived(PositionSaveData data)
    {
        if (activeRenders.TryGetValue(data.ID, out RenderPath render))
        {
            Destroy(render.gameObject);
            activeRenders.Remove(data.ID);
        }

        List<Vector3> linePositions = new();
        foreach (var pos in data.AnchoredPositions)
        {
            if (SSA.Anchors.TryGetValue(Guid.Parse(pos.AnchorUUID), out SpatialAnchor spatialAnchor))
            {
                linePositions.Add(spatialAnchor.transform.TransformPoint(pos.RelativePosition));
            }
            else
            {
                Debug.LogError($"Anchor with UUID {pos.AnchorUUID} not found.");
            }
        }

        if (linePositions.Count < 2)
        {
            Debug.LogError("Not enough positions to render.");
            return;
        }

        GameObject obj = new($"PathRenderer({data.ID})");
        RenderPath path = obj.AddComponent<RenderPath>();
        path.RenderLine(linePositions);
        activeRenders.Add(data.ID, path);
    }
    #endregion

    [ClientCallback]
    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<PositionSaveData>(OnTrackingDataReceived);
    }

    [ClientCallback]
    public override void OnStopClient()
    {
        base.OnStopClient();
        NetworkClient.UnregisterHandler<PositionSaveData>();
    }

    public void ToggleLines(bool value)
    {
        foreach (var line in activeRenders.Values)
        {
            line.ToggleLine(value);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdStopTracking()
    {
        handler.StopAllTrackers();
        manager.SaveData();
    }

    [Command(requiresAuthority = false)]
    public void CmdVisualizeLines()
    {
        handler.RenderAllSavedPaths();
        manager.VisualizeOnClients();
    }

    [Command(requiresAuthority = false)]
    public void CmdStartTracking()
    {
        handler.StartAllTrackers();
    }
}