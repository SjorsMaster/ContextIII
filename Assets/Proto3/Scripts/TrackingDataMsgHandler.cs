using Mirror;
using SharedSpaces;
using SharedSpaces.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDataMsgHandler : NetworkBehaviour
{
    private readonly List<RenderPath> activeRenders = new();

    #region Event Handlers
    private void OnTrackingDataReceived(PositionSaveData data)
    {
        foreach (var render in activeRenders)
        {
            Destroy(render.gameObject);
        }

        activeRenders.Clear();

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
        activeRenders.Add(path);
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
}