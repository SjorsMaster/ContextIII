using Mirror;
using System;
using UnityEngine;
using SharedSpaces.NetworkSingletons;

namespace ContextIII
{
    public class RelativeSource : NetworkSingleton<RelativeSource>
    {
        [SyncVar, HideInInspector] private Vector3 serverPivotPosData;
        [SyncVar, HideInInspector] private Quaternion serverPivotRotData;

        public Vector3 ServerPivotPosData => serverPivotPosData;
        public Quaternion ServerPivotRotData => serverPivotRotData;

        [SerializeField] private TrackPositionAndRotationSelector TrackPositionAndRotationSelector;

        public event Action<Vector3, Quaternion> OnTransformChanged;

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        [ServerCallback]
        private void Update()
        {
            serverPivotPosData = transform.position;
            serverPivotRotData = transform.rotation;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            position = new Vector3(
                TrackPositionAndRotationSelector.TrackXPosition ? position.x : Position.x,
                TrackPositionAndRotationSelector.TrackYPosition ? position.y : Position.y,
                TrackPositionAndRotationSelector.TrackZPosition ? position.z : Position.z);

            rotation = Quaternion.Euler(
                TrackPositionAndRotationSelector.TrackXRotation ? rotation.eulerAngles.x : Rotation.eulerAngles.x,
                TrackPositionAndRotationSelector.TrackYRotation ? rotation.eulerAngles.y : Rotation.eulerAngles.y,
                TrackPositionAndRotationSelector.TrackZRotation ? rotation.eulerAngles.z : Rotation.eulerAngles.z);

            transform.SetPositionAndRotation(position, rotation);
            OnTransformChanged?.Invoke(position, rotation);
        }
    }
}
