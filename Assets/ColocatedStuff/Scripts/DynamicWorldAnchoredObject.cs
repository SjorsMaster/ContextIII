using Mirror;
using PortalsVR;
using SharedSpaces;
using SharedSpaces.Managers;
using System.Threading.Tasks;
using UnityEngine;

public class DynamicWorldAnchoredObject : DynamicAnchoredObject
{
    [SerializeField] private bool doMigration = true;

    [SerializeField] private SearchClosestWorldAnchor searchClosestWorldAnchor;
    [SerializeField] private PortalTraveller portalTraveller;

    private WorldsAnchorManager worldsAnchorManager;

    [SerializeField] private bool debugMe = false;

    #region Event Handlers
    public void PortalTraveller_OnWorldChanged(string newWorld)
    {
        searchClosestWorldAnchor.ForceCheck();
    }

    protected override void OnAnchorUUIDUpdated(string oldValue, string newValue)
    {
        base.OnAnchorUUIDUpdated(oldValue, newValue);

        UpdateWorldWhenNotBusy(oldValue, newValue);
    }
    #endregion

    [ClientCallback]
    private void Start()
    {
        if (!doMigration)
        {
            return;
        }

        if (portalTraveller != null)
        {
            World.worlds[portalTraveller.activeWorld].Add(gameObject, false);
        }

        if (!debugMe) return;
        InputManager.LPrimaryPress += () =>
        {
            VRDebugPanel.Instance.SendDebugMessage($"Visuals are active: {Visuals.activeSelf}");
        };
    }

    [ServerCallback]
    protected override void ServerOnEnable()
    {
        base.ServerOnEnable();
        if (portalTraveller != null)
        {
            portalTraveller.onWorldChanged += PortalTraveller_OnWorldChanged;
        }
    }

    [ServerCallback]
    protected override void ServerOnDisable()
    {
        base.ServerOnDisable();

        if (portalTraveller != null)
        {
            portalTraveller.onWorldChanged -= PortalTraveller_OnWorldChanged;
        }
    }

    [ClientCallback]
    protected override void ClientOnEnable()
    {
        base.ClientOnEnable();

        searchClosestWorldAnchor.enabled = false;

        if (portalTraveller != null)
        {
            portalTraveller.enabled = false;
        }
    }

    private async Task<bool> Cache()
    {
        float time = Time.time;
        float timeout = 10f;
        while (worldsAnchorManager == null)
        {
            worldsAnchorManager = WorldsAnchorManager.TryGetInstance();

            if (Time.time > time + timeout)
            {
                return false;
            }

            await Task.Yield();
        }

        return true;
    }

    private string oldAnchorUUID;

    public async void CorrectToWorld()
    {
        if (!doMigration)
        {
            return;
        }

        bool result = await Cache();

        if (!result)
        {
            throw new System.Exception("Failed to cache WorldsAnchorManager.");
        }

        if (!worldsAnchorManager.ReferenceWorld.TryGetValue(AnchorUUID, out string targetWorld))
        {
            throw new System.Exception("Failed to get target world.");
        }

        if (targetWorld == "Global")
        {
            return;
        }

        if (worldsAnchorManager.ReferenceWorld.TryGetValue(oldAnchorUUID, out string oldWorld))
        {
            if (targetWorld == oldWorld)
            {
                return;
            }

            World.worlds[oldWorld].Migrate(gameObject, World.worlds[targetWorld], false);
        }
        else
        {
            World.worlds[targetWorld].Add(gameObject, false);
        }

        oldAnchorUUID = AnchorUUID;
    }

    private async void UpdateWorldWhenNotBusy(string oldValue, string newValue)
    {
        if (!doMigration)
        {
            return;
        }

        bool result = await Cache();

        if (!result)
        {
            throw new System.Exception("Failed to cache WorldsAnchorManager.");
        }

        while (worldsAnchorManager.IsBusy)
        {
            await Task.Yield();
        }

        if (!worldsAnchorManager.ReferenceWorld.TryGetValue(newValue, out string targetWorld))
        {
            throw new System.Exception("Failed to get target world.");
        }

        if (targetWorld == "Global")
        {
            return;
        }

        if (worldsAnchorManager.ReferenceWorld.TryGetValue(oldValue, out string oldWorld))
        {
            if (targetWorld == oldWorld)
            {
                return;
            }

            World.worlds[oldWorld].Migrate(gameObject, World.worlds[targetWorld], false);
        }
        else
        {
            World.worlds[targetWorld].Add(gameObject, false);
        }
    }
}
