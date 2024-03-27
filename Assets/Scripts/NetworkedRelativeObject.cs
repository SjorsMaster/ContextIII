using Mirror;
using UnityEngine;

namespace ContextIII
{
    public class NetworkedRelativeObject : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnChangedPositionData))] private Vector3 positionData;
        [SyncVar(hook = nameof(OnChangedRotationData))] private Quaternion rotationData;

        private Vector3 currentPivotPos;
        private Quaternion currentPivotRot;

        private void Update()
        {
            if (isServer && syncDirection == SyncDirection.ServerToClient)
            {
                positionData = transform.position - currentPivotPos;
                rotationData = Quaternion.Inverse(currentPivotRot) * transform.rotation;
            }
            else if (isClient && syncDirection == SyncDirection.ClientToServer)
            {
                (Vector3 calculatedPos, Quaternion calculatedRotation) = CalculateRelativeToNewOrigin(
                    transform.position,
                    transform.rotation,
                    currentPivotPos,
                    currentPivotRot,
                    RelativeSource.Instance.ServerPivotPosData,
                    RelativeSource.Instance.ServerPivotRotData);
                positionData = calculatedPos;
                rotationData = calculatedRotation;
            }
        }

        private void OnEnable()
        {
            RelativeSource.Instance.OnTransformChanged += RelativeSource_OnChangedRelativeSource;
            transform.SetPositionAndRotation(RelativeSource.Instance.Position + positionData, rotationData * RelativeSource.Instance.Rotation);

            currentPivotPos = RelativeSource.Instance.Position;
            currentPivotRot = RelativeSource.Instance.Rotation;
        }

        private void OnDisable()
        {
            RelativeSource.Instance.OnTransformChanged -= RelativeSource_OnChangedRelativeSource;
        }

        private void OnChangedPositionData(Vector3 oldPosition, Vector3 newPostion)
        {
            transform.position = currentPivotPos + currentPivotRot * newPostion;
        }

        private void OnChangedRotationData(Quaternion oldRotation, Quaternion newRotation)
        {
            // We assign euler angles to avoid gimbal lock.
            transform.eulerAngles = newRotation.eulerAngles + currentPivotRot.eulerAngles;
        }

        private void RelativeSource_OnChangedRelativeSource(Vector3 newPivotPos, Quaternion newPivotRot)
        {
            (Vector3 calculatedPos, Quaternion calculatedRotation) = CalculateRelativeToNewOrigin(
                transform.position,
                transform.rotation,
                currentPivotPos,
                currentPivotRot,
                newPivotPos,
                newPivotRot);

            transform.SetPositionAndRotation(calculatedPos, calculatedRotation);

            currentPivotPos = newPivotPos;
            currentPivotRot = newPivotRot;
        }

        // Transforming a point connected to a pivot to a new pivot, including it's rotation.
        // Pivot PO = Pivot Old
        // Pivot PN = Pivot New
        // P = Point relative to old pivot
        // P` = Point relative to new pivot
        // Q = Quaternion that rotates PO to PN
        // P` = Q * (P - PO) + PN
        private (Vector3, Quaternion) CalculateRelativeToNewOrigin(
            Vector3 tartgetPos,
            Quaternion tartgetRot,
            Vector3 oldOriginPos,
            Quaternion oldOriginRot,
            Vector3 newOriginPos,
            Quaternion newOriginRot)
        {
            Quaternion oldToNewRot = newOriginRot * Quaternion.Inverse(oldOriginRot);
            Vector3 newPosition = oldToNewRot * (tartgetPos - oldOriginPos) + newOriginPos;
            Quaternion newRotation = tartgetRot * oldToNewRot;

            return (newPosition, newRotation);
        }
    }
}
