using UnityEngine;

namespace ContextIII
{
    public class LocalTrackedDevice : RegulatorSingleton<LocalTrackedDevice>
    {
        [field: SerializeField] public OVRCameraRig CameraRig { get; private set; }
        [field: SerializeField] public Transform CentreAnchorEyeTransform { get; private set; }
        [field: SerializeField] public Transform LeftAnchorTransform { get; private set; }
        [field: SerializeField] public Transform RightAnchorTransform { get; private set; }
    }
}
