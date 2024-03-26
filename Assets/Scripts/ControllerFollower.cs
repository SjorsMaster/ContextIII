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

        Head.SetPositionAndRotation(trackedDevice.CentreEyeAnchor.position, trackedDevice.CentreEyeAnchor.rotation);
        LeftHand.SetPositionAndRotation(trackedDevice.LeftHandAnchor.position, trackedDevice.LeftHandAnchor.rotation);
        RightHand.SetPositionAndRotation(trackedDevice.RightHandAnchor.position, trackedDevice.RightHandAnchor.rotation);
    }
}
