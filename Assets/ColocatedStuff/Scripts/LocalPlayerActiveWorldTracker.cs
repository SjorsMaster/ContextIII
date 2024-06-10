using Mirror;
using PortalsVR;
using SharedSpaces.Managers;
using UnityEngine;

public class LocalPlayerActiveWorldTracker : NetworkBehaviour
{
    [SerializeField] private PortalTraveller networkedPortalTraveller;
    [SerializeField] private DynamicWorldAnchoredObject dynamicWorldAnchoredObject;

    [SyncVar(hook = nameof(OnActiveWorldChanged))] private string activeWorld;

    private PortalTraveller centerEyeAnchorPortalTraveller; // Local client only.

    #region Event Handlers
    private void CenterEyeAnchorPortalTraveller_OnWorldChanged(string targetWorld)
    {
        if (activeWorld == targetWorld)
        {
            return;
        }

        activeWorld = targetWorld;
    }

    private void OnActiveWorldChanged(string oldValue, string newValue)
    {
        networkedPortalTraveller.ForcedTeleport(newValue);
    }
    #endregion

    [ClientCallback]
    private void Awake()
    {
        centerEyeAnchorPortalTraveller = TrackedObjectsManager.Instance.CenterEyeAnchor.gameObject.GetComponent<PortalTraveller>();

        syncDirection = SyncDirection.ClientToServer;

        activeWorld = centerEyeAnchorPortalTraveller.activeWorld;
    }

    [ClientCallback]
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        centerEyeAnchorPortalTraveller.onWorldChanged += CenterEyeAnchorPortalTraveller_OnWorldChanged;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        centerEyeAnchorPortalTraveller.onWorldChanged -= CenterEyeAnchorPortalTraveller_OnWorldChanged;
    }
}