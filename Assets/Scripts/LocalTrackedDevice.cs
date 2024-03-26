using UnityEngine;

namespace ContextIII
{
    public class LocalTrackedDevice : RegulatorSingleton<LocalTrackedDevice>
    {
        [field: SerializeField] public OVRCameraRig CameraRig { get; private set; }

        [field: SerializeField] public Transform CentreEyeAnchor { get; private set; }
        [field: SerializeField] public Transform LeftHandAnchor { get; private set; }
        [field: SerializeField] public Transform RightHandAnchor { get; private set; }
    }
}
