using Mirror;
using PortalsVR;
using SharedSpaces;
using SharedSpaces.Data;
using System.Collections.Generic;
using UnityEngine;

public class SearchClosestWorldAnchor : SearchClosestAnchor
{
    [SerializeField] private PortalTraveller portalTraveller;

    private WorldsAnchorManager worldsAnchorManager;

    protected override bool CompareAnchors(out ReferenceAnchorData closestAnchor, IEnumerable<ReferenceAnchorData> anchorDatas)
    {
        closestAnchor = default;
        bool hasValue = false;

        foreach (ReferenceAnchorData anchor in anchorDatas)
        {
            if (anchor.SpatialAnchor == null)
            {
                continue;
            }

            if (!Cache())
            {
                return false;
            }

            if (!worldsAnchorManager.ReferenceWorld.TryGetValue(anchor.UUID, out string worldName))
            {
                continue;
            }

            if (worldName != portalTraveller.activeWorld)
            {
                continue;
            }

            if (!hasValue)
            {
                closestAnchor = anchor;
                hasValue = true;
                continue;
            }

            float distanceToClosestAnchor = Vector3.Distance(closestAnchor.SpatialAnchor.transform.position, transform.position);
            float distanceToCurrentAnchor = Vector3.Distance(anchor.SpatialAnchor.transform.position, transform.position);
            if (distanceToCurrentAnchor < distanceToClosestAnchor)
                closestAnchor = anchor;
        }

        return hasValue;
    }

    private bool Cache()
    {
        if (worldsAnchorManager == null)
        {
            return worldsAnchorManager = WorldsAnchorManager.TryGetInstance();
        }

        return true;
    }
}