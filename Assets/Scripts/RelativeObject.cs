using System;
using UnityEngine;

namespace ContextIII
{
    public class RelativeObject : MonoBehaviour
    {
        [SerializeField] private CalculationType calculationType = CalculationType.NonNetworked;

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEuler { get; private set; }

        private void OnEnable()
        {
            StartPosition = transform.position;
            StartEuler = transform.eulerAngles;

            SourceOrigin sourceOrigin = SourceOrigin.Instance;
            sourceOrigin.OnRecalculateRelativeTransforms += SourceOrigin_OnRecalculateRelativeTransform;
        }

        private void OnDisable()
        {
            transform.position = StartPosition;
            transform.eulerAngles = StartEuler;

            SourceOrigin soureOrigin = SourceOrigin.Instance;
            soureOrigin.OnRecalculateRelativeTransforms -= SourceOrigin_OnRecalculateRelativeTransform;
        }

        public void SourceOrigin_OnRecalculateRelativeTransform(
            CalculationType calculationType,
            Origin oldOrigin, 
            Origin newOrigin)
        {
            if (calculationType != CalculationType.All && calculationType != this.calculationType)
                return;

            Vector3 directionStartToOldOrigin = oldOrigin.Position - StartPosition;
            Vector3 eulerOldOriginToNewOrigin = newOrigin.Eulers - oldOrigin.Eulers;
            Vector3 directionOldOriginToNewOrigin = newOrigin.Position - oldOrigin.Position;
            Vector3 directionStartToOldOriginRotated = Quaternion.Euler(eulerOldOriginToNewOrigin) * -directionStartToOldOrigin;

            Vector3 result = StartPosition
                + directionStartToOldOrigin
                + directionOldOriginToNewOrigin
                + directionStartToOldOriginRotated;

            SourceOrigin sourceOrigin = SourceOrigin.Instance;
            TrackPositionAndRotationSelector selector = sourceOrigin.TrackPositionAndRotationSelector;

            result.x = selector.TrackXPosition ? result.x : StartPosition.x;
            result.y = selector.TrackYPosition ? result.y : StartPosition.y;
            result.z = selector.TrackZPosition ? result.z : StartPosition.z;
            transform.position = result;

            eulerOldOriginToNewOrigin.x = selector.TrackXRotation ? eulerOldOriginToNewOrigin.x : 0;
            eulerOldOriginToNewOrigin.y = selector.TrackYRotation ? eulerOldOriginToNewOrigin.y : 0;
            eulerOldOriginToNewOrigin.z = selector.TrackZRotation ? eulerOldOriginToNewOrigin.z : 0;
            transform.eulerAngles = StartEuler + eulerOldOriginToNewOrigin;
            
        }
    }
}