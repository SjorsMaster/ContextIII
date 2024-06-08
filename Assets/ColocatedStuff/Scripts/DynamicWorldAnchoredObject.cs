using Mirror;
using PortalsVR;
using SharedSpaces;
using System.Collections;
using UnityEngine;

public class DynamicWorldAnchoredObject : DynamicAnchoredObject
{
    [SerializeField] private SearchClosestWorldAnchor searchClosestWorldAnchor;
    [SerializeField] private PortalTraveller portalTraveller;

    private string previousWorld;

    #region Event Handlers
    public void PortalTraveller_OnWorldChanged(string newWorld)
    {
        if (!string.IsNullOrEmpty(previousWorld))
        {
            World.worlds[previousWorld].Migrate(gameObject, World.worlds[newWorld], false);
        }

        searchClosestWorldAnchor.ForceCheck();
    }
    #endregion

    [ClientCallback]
    private void Start()
    {
        World.worlds[portalTraveller.activeWorld].Add(gameObject);
    }

    protected override void ClientOnEnable()
    {
        base.ClientOnEnable();
        portalTraveller.onWorldChanged += PortalTraveller_OnWorldChanged;
    }

    protected override void ClientOnDisable()
    {
        base.ClientOnDisable();
        portalTraveller.onWorldChanged -= PortalTraveller_OnWorldChanged;
    }
}
