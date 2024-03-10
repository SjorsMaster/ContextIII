using UnityEngine;

namespace ContextIII
{
    [System.Serializable]
    public class TrackPositionAndRotationSelector
    {
        [field: SerializeField] public bool TrackXPosition { get; private set; } = true;
        [field: SerializeField] public bool TrackYPosition { get; private set; } = true;
        [field: SerializeField] public bool TrackZPosition { get; private set; } = true;
        [field: SerializeField] public bool TrackXRotation { get; private set; } = false;
        [field: SerializeField] public bool TrackYRotation { get; private set; } = true;
        [field: SerializeField] public bool TrackZRotation { get; private set; } = false;
    }
}