using Mirror;
using PortalsVR;
using SharedSpaces.Managers;

public class LocalPlayerActiveWorldTracker : NetworkBehaviour
{
    private PortalTraveller centerEyeAnchorPortalTraveler;
    private DynamicWorldAnchoredObject dynamicWorldAnchoredObject;

    #region Event Handlers
    private void DynamicWorldAnchoredObject_OnWorldChanged(string targetWorld)
    {
        centerEyeAnchorPortalTraveler.ForcedTeleport(targetWorld);
    }
    #endregion

    [ClientCallback]
    private void Awake()
    {
        centerEyeAnchorPortalTraveler = TrackedObjectsManager.Instance.CenterEyeAnchor.gameObject.GetComponent<PortalTraveller>();
        dynamicWorldAnchoredObject = GetComponent<DynamicWorldAnchoredObject>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        dynamicWorldAnchoredObject.OnWorldChanged += DynamicWorldAnchoredObject_OnWorldChanged;
    }

    public override void OnStopLocalPlayer()
    {
        base.OnStopLocalPlayer();
        dynamicWorldAnchoredObject.OnWorldChanged -= DynamicWorldAnchoredObject_OnWorldChanged;
    }
}