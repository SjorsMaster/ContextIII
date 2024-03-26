using System;
using UnityEngine;

namespace ContextIII
{
    /// <summary>
    /// You can place this script on an empty GameObject in the scene to act as the source of the relative transforms.
    /// </summary>
    public class SourceOrigin : Singleton<SourceOrigin>
    {
        public event Action<RelativeType, Transform, Transform> OnRecalculateRelativeTransforms;

        [field: SerializeField] public TrackPositionAndRotationSelector TrackPositionAndRotationSelector { get; private set; }
        [field: SerializeField] public Transform RelativeOrigin { get; private set; }

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEulers { get; private set; }

        private void Start()
        {
            StartPosition = transform.position;
            StartEulers = transform.eulerAngles;

            LocalTrackedDevice localTrackedDevice = LocalTrackedDevice.Instance;
            RelativeOrigin.position = new Vector3(
                TrackPositionAndRotationSelector.TrackXPosition ? localTrackedDevice.LeftHandAnchor.transform.position.x : RelativeOrigin.position.x,
                TrackPositionAndRotationSelector.TrackYPosition ? localTrackedDevice.LeftHandAnchor.transform.position.y : RelativeOrigin.position.y,
                TrackPositionAndRotationSelector.TrackZPosition ? localTrackedDevice.LeftHandAnchor.transform.position.z : RelativeOrigin.position.z);
            RelativeOrigin.eulerAngles = new Vector3(
                TrackPositionAndRotationSelector.TrackXRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.x : RelativeOrigin.eulerAngles.x,
                TrackPositionAndRotationSelector.TrackYRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.y : RelativeOrigin.eulerAngles.y,
                TrackPositionAndRotationSelector.TrackZRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.z : RelativeOrigin.eulerAngles.z);
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                Debug.LogWarning("Pressed the primary index trigger on the right touch controller.");
                RecalculateRelativeTransformsToLeftController(RelativeType.Stationary, true);
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            {
                Debug.LogWarning("Pressed the primary index trigger on the left touch controller.");
                {
                    foreach (RelativeObject v in FindObjectsByType<RelativeObject>(FindObjectsSortMode.None))
                    {
                        if (v.enabled)
                            continue;

                        v.enabled = true;
                    }
                }
            }
        }

        public void RecalculateRelativeTransformsToLeftController(RelativeType targetRelativeType, bool SetRelativeOrigin = false)
        {
            LocalTrackedDevice localTrackedDevice = LocalTrackedDevice.Instance;
            if (SetRelativeOrigin)
            {
                RelativeOrigin.position = new Vector3(
                    TrackPositionAndRotationSelector.TrackXPosition ? localTrackedDevice.LeftHandAnchor.transform.position.x : RelativeOrigin.position.x,
                    TrackPositionAndRotationSelector.TrackYPosition ? localTrackedDevice.LeftHandAnchor.transform.position.y : RelativeOrigin.position.y,
                    TrackPositionAndRotationSelector.TrackZPosition ? localTrackedDevice.LeftHandAnchor.transform.position.z : RelativeOrigin.position.z);
                RelativeOrigin.eulerAngles = new Vector3(
                    TrackPositionAndRotationSelector.TrackXRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.x : RelativeOrigin.eulerAngles.x,
                    TrackPositionAndRotationSelector.TrackYRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.y : RelativeOrigin.eulerAngles.y,
                    TrackPositionAndRotationSelector.TrackZRotation ? localTrackedDevice.LeftHandAnchor.transform.eulerAngles.z : RelativeOrigin.eulerAngles.z);
            }

            OnRecalculateRelativeTransforms?.Invoke(
                targetRelativeType,
                transform,
                RelativeOrigin);
        }

        public void RecalculateRelativeTransformsToLeftControllerButton()
        {
            RecalculateRelativeTransformsToLeftController(RelativeType.Stationary, true);
        }
    }
}
