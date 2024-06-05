using SharedSpaces;
using SharedSpaces.Data;
using SharedSpaces.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] private float interval = .25f;
    [SerializeField] private AnchoredObject anchoredObject;

    public List<AnchoredPosition> PositionData { get; private set; } = new();

    private bool track;

    private void Awake()
    {
        TrackingSaveDataHandler.PositionTrackers.Add(this);
    }

    public void StartTracking()
    {
        track = true;
        PositionData.Clear();
        StartCoroutine(TrackPositions());
    }

    public void StopTracking()
    {
        track = false;
    }

    private IEnumerator TrackPositions()
    {
        ServerManager manager = ServerManager.TryGetInstance();
        if (manager == null)
        {
            Debug.LogError("ServerManager not found.");
            yield break;
        }

        while (track)
        {
            if (manager.ReferenceAnchors.TryGetValue(anchoredObject.AnchorUUID, out ReferenceAnchorData data))
            {
                PositionData.Add(new()
                {
                    AnchorUUID = anchoredObject.AnchorUUID,
                    RelativePosition = data.SpatialAnchor.transform.InverseTransformPoint(transform.position)
                });
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
