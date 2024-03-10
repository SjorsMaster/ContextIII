using System;
using UnityEngine;

namespace ContextIII
{
    /// <summary>
    /// You can place this script on an empty GameObject in the scene to act as the source of the relative transforms.
    /// </summary>
    public class SourceOrigin : Singleton<SourceOrigin>
    {
        public event Action<PositionType, Transform, Transform> OnRecalculateRelativeTransforms;

        [field: SerializeField] public TrackPositionAndRotationSelector TrackPositionAndRotationSelector { get; private set; }
        [field: SerializeField] public Transform RelativeOrigin { get; private set; }

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEulers { get; private set; }

        private void Start()
        {
            StartPosition = transform.position;
            StartEulers = transform.eulerAngles;

            LocalTrackedDevice localTrackedDevice = LocalTrackedDevice.Instance;
            RelativeOrigin.position = localTrackedDevice.LeftAnchorTransform.position;
            RelativeOrigin.eulerAngles = localTrackedDevice.LeftAnchorTransform.eulerAngles;
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                RecalculateAllRelativeTransforms();
        }

        public void RecalculateAllRelativeTransforms()
        {
            LocalTrackedDevice localTrackedDevice = LocalTrackedDevice.Instance;
            OnRecalculateRelativeTransforms?.Invoke(
                PositionType.NonNetworked,
                transform,
                localTrackedDevice.LeftAnchorTransform);

            RelativeOrigin.position = localTrackedDevice.LeftAnchorTransform.position;
            RelativeOrigin.eulerAngles = localTrackedDevice.LeftAnchorTransform.eulerAngles;
        }
    }
}
