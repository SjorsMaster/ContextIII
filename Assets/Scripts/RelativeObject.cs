using UnityEngine;

namespace ContextIII
{
    public class RelativeObject : MonoBehaviour
    {
        [SerializeField] private RelativeType relativeType = RelativeType.Stationary;

        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEuler { get; private set; }

        /// <summary>
        /// This is going to be used to send the relative position and rotation to the server.
        /// </summary>
        public (Vector3, Vector3) RelativeToSource { 
            get
            {
                return CalculateRelativeToNewOrigin(
                    transform,
                    SourceOrigin.Instance.RelativeOrigin,
                    SourceOrigin.Instance.transform);
            }
        }

        private void OnEnable()
        {
            StartPosition = transform.position;
            StartEuler = transform.eulerAngles;

            SourceOrigin sourceOrigin = SourceOrigin.Instance;
            sourceOrigin.OnRecalculateRelativeTransforms += TransformRelativeToNewOrigin;
        }

        private void OnDisable()
        {
            transform.position = StartPosition;
            transform.eulerAngles = StartEuler;

            SourceOrigin soureOrigin = SourceOrigin.Instance;
            soureOrigin.OnRecalculateRelativeTransforms -= TransformRelativeToNewOrigin;
        }

        /// <summary>
        /// Calculates the position relative to the new origin and sets the position and rotation of the object.
        /// </summary>
        /// <param name="relativeType"></param>
        /// <param name="oldOrigin"></param>
        /// <param name="newOrigin"></param>
        /// <remarks>Only works for position and euler rotation.</remarks>
        private void TransformRelativeToNewOrigin(
            RelativeType relativeType,
            Transform oldOrigin,
            Transform newOrigin)
        {
            if (relativeType != this.relativeType)
                return;

            transform.position = StartPosition;
            transform.eulerAngles = StartEuler;

            (Vector3 newPosition, Vector3 newEuler) = CalculateRelativeToNewOrigin(
                transform,
                oldOrigin,
                newOrigin);

            SourceOrigin sourceOrigin = SourceOrigin.Instance;
            TrackPositionAndRotationSelector selector = sourceOrigin.TrackPositionAndRotationSelector;

            newPosition.x = selector.TrackXPosition ? newPosition.x : transform.position.x;
            newPosition.y = selector.TrackYPosition ? newPosition.y : transform.position.y;
            newPosition.z = selector.TrackZPosition ? newPosition.z : transform.position.z;

            newEuler.x = selector.TrackXRotation ? newEuler.x : 0;
            newEuler.y = selector.TrackYRotation ? newEuler.y : 0;
            newEuler.z = selector.TrackZRotation ? newEuler.z : 0;

            transform.position = newPosition;
            transform.eulerAngles = StartEuler + newEuler;
        }

        /// <summary>
        /// A method that calculates the position and rotation of the current object relative to the new origin.
        /// </summary>
        /// <param name="oldOrigin"></param>
        /// <param name="newOrigin"></param>
        /// <returns>Item 1: A Vector3 with the position, Item 2: A vector with the euler rotation.</returns>
        /// <remarks>Only works for position and euler rotation.</remarks>
        private (Vector3, Vector3) CalculateRelativeToNewOrigin(
            Transform current,
            Transform oldOrigin,
            Transform newOrigin)
        {
            Vector3 newEuler = newOrigin.eulerAngles - oldOrigin.eulerAngles;
            Vector3 directionOldToCurrentRotated = Quaternion.Euler(newEuler) * (current.position - oldOrigin.position);
            Vector3 newPosition = newOrigin.position + directionOldToCurrentRotated;

            return (newPosition, newEuler);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            if (relativeType != RelativeType.Moving_SendToServer)
                return;

            Gizmos.color = Color.red;

            (Vector3 relativePosition, Vector3 relativeEuler) = RelativeToSource;
            float yRotDifference = transform.eulerAngles.y + relativeEuler.y;
            Gizmos.DrawSphere(relativePosition, 0.3f);
            Gizmos.DrawLine(relativePosition, relativePosition + Quaternion.Euler(0, yRotDifference, 0) * Vector3.forward * 5f);
        }
    }
}
