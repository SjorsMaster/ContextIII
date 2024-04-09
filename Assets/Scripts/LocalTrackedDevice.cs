using UnityEngine;
using SharedSpaces.Singletons;

namespace ContextIII
{
    public class LocalTrackedDevice : RegulatorSingleton<LocalTrackedDevice>
    {
        [field: SerializeField] public OVRCameraRig CameraRig { get; private set; }

        [field: SerializeField] public RelativeObject CentreAnchorEyeRelative { get; private set; }
        [field: SerializeField] public RelativeObject LeftAnchorRelative { get; private set; }
        [field: SerializeField] public RelativeObject RightAnchorRelative { get; private set; }
    }
}
