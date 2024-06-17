using SharedSpaces.Data;
using SharedSpaces.Managers;
using SharedSpaces.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public class TrackingSaveDataHandler : MonoBehaviour, ISaveDataHandler<TrackingSaveData>
{
    public static List<PositionTracker> PositionTrackers = new();

    private readonly List<RenderPath> activeRenders = new();

    private void Awake()
    {
        TrackingSaveDataManager.SaveDataList.Add(this);
    }

    public void LoadData(TrackingSaveData data)
    {
    }

    public void NewData()
    {
    }

    public void SaveData(ref TrackingSaveData data)
    {
        List<PositionTracker> positionTrackers = PositionTrackers;
        List<PositionSaveData> positionSaveDatas = new(data.PositionSaveDatas);

        for (uint i = 0; i < positionTrackers.Count; i++)
        {
            positionSaveDatas.Add(new()
            {
                ID = i,
                AnchoredPositions = positionTrackers[(int)i].PositionData
            });
        }

        data.PositionSaveDatas = positionSaveDatas.ToArray();
    }

    public void RenderAllSavedPaths()
    {
        foreach (var render in activeRenders)
        {
            Destroy(render.gameObject);
        }

        activeRenders.Clear();

        ServerManager manager = ServerManager.TryGetInstance();
        if (manager != null)
        {
            foreach (var data in TrackingSaveDataManager.Data.PositionSaveDatas)
            {
                List<Vector3> linePositions = new();
                foreach (var pos in data.AnchoredPositions)
                {
                    if (manager.ReferenceAnchors.TryGetValue(pos.AnchorUUID, out ReferenceAnchorData referenceAnchor))
                    {
                        linePositions.Add(referenceAnchor.SpatialAnchor.transform.TransformPoint(pos.RelativePosition));
                    }
                    else
                    {
                        Debug.LogError($"Anchor with UUID {pos.AnchorUUID} not found.");
                    }
                }

                if (linePositions.Count < 2)
                {
                    Debug.LogError("Not enough positions to render.");
                    continue;
                }

                GameObject obj = new($"PathRenderer({data.ID})");
                RenderPath path = obj.AddComponent<RenderPath>();
                path.RenderLine(linePositions);
                activeRenders.Add(path);
            }
        }
    }

    public void StartAllTrackers()
    {
        foreach (var tracker in PositionTrackers)
        {
            tracker.StartTracking();
        }
    }

    public void StopAllTrackers()
    {
        foreach (var tracker in PositionTrackers)
        {
            tracker.StopTracking();
        }
    }
}
