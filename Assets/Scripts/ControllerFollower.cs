using ContextIII;
using Mirror;
using UnityEngine;

public class ControllerFollower : NetworkBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private Transform LeftHand;
    [SerializeField] private Transform RightHand;

    [ClientCallback]
    private void Update()
    {
        LocalTrackedDevice trackedDevice = LocalTrackedDevice.Instance;
        if (!trackedDevice) 
            return;

        Head.SetPositionAndRotation(trackedDevice.CentreAnchorEyeRelative.transform.position, trackedDevice.CentreAnchorEyeRelative.transform.rotation);
        LeftHand.SetPositionAndRotation(trackedDevice.LeftAnchorRelative.transform.position, trackedDevice.LeftAnchorRelative.transform.rotation);
        RightHand.SetPositionAndRotation(trackedDevice.RightAnchorRelative.transform.position, trackedDevice.RightAnchorRelative.transform.rotation);
    }
}
