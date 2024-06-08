using Mirror;
using PortalsVR;
using SharedSpaces;
using SharedSpaces.Managers;
using System;
using UnityEngine;

public class DynamicWorldAnchoredObject : DynamicAnchoredObject
{
    [SerializeField] private SearchClosestWorldAnchor searchClosestWorldAnchor;
    [SerializeField] private PortalTraveller portalTraveller;

    private WorldsAnchorManager worldsAnchorManager;

    private string currentWorld; // Client only.

    #region Event Handlers
    public void PortalTraveller_OnWorldChanged(string newWorld)
    {
        searchClosestWorldAnchor.ForceCheck();
    }

    protected override void OnAnchorUUIDUpdated(string oldValue, string newValue)
    {
        base.OnAnchorUUIDUpdated(oldValue, newValue);
        if (!Cache())
        {
            throw new System.Exception("Failed to cache WorldsAnchorManager.");
        }

        if (string.IsNullOrEmpty(newValue) && worldsAnchorManager.ReferenceWorld.TryGetValue(newValue, out string targetWorld))
        {
            World.worlds[currentWorld].Migrate(gameObject, World.worlds[targetWorld], false);

            currentWorld = targetWorld;
        }
    }
    #endregion

    [ClientCallback]
    private void Start()
    {
        World.worlds[portalTraveller.activeWorld].Add(gameObject);

        currentWorld = portalTraveller.activeWorld;
    }

    [ServerCallback]
    protected override void ServerOnEnable()
    {
        base.ServerOnEnable();
        portalTraveller.onWorldChanged += PortalTraveller_OnWorldChanged;
    }

    [ServerCallback]
    protected override void ServerOnDisable()
    {
        base.ServerOnDisable();
        portalTraveller.onWorldChanged -= PortalTraveller_OnWorldChanged;
    }

    [ClientCallback]
    protected override void ClientOnEnable()
    {
        base.ClientOnEnable();

        searchClosestWorldAnchor.enabled = false;
        portalTraveller.enabled = false;
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
