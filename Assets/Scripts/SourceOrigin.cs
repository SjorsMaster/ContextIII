using System;
using UnityEngine;

namespace ContextIII
{
    /// <summary>
    /// You can place this script on an empty GameObject in the scene to act as the source of the relative transforms.
    /// </summary>
    public class SourceOrigin : Singleton<SourceOrigin>
    {
        public event Action<CalculationType, Origin, Origin> OnRecalculateRelativeTransforms;

        [field: SerializeField] public TrackPositionAndRotationSelector TrackPositionAndRotationSelector { get; private set; }

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEulers { get; private set; }

        private void Start()
        {
            StartPosition = transform.position;
            StartEulers = transform.eulerAngles;
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                RecalculateAllRelativeTransforms();
        }

        public void RecalculateAllRelativeTransforms()
        {
            Transform leftController = LocalTrackedDevice.Instance.LeftAnchorTransform;

            Origin thisOrigin = new(StartPosition, StartEulers);
            Origin leftControllerOrigin = new(leftController.position, leftController.eulerAngles);

            OnRecalculateRelativeTransforms?.Invoke(
                CalculationType.All, 
                thisOrigin, 
                leftControllerOrigin);
        }
    }
}