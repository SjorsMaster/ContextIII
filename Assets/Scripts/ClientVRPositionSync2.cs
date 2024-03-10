using Mirror;
using UnityEngine;

namespace ContextIII
{
    // Each local client sends its headset and hand data to the server, which then sends it to all remote clients
    public class ClientVRPositionSync2 : NetworkBehaviour
    {
        [SerializeField] private LocalTrackedDevice localTrackedDevicePrefab;

        [SerializeField] private Transform headObject;
        [SerializeField] private Transform leftHandObject;
        [SerializeField] private Transform rightHandObject;

        [SyncVar] private Vector3 headPosition;
        [SyncVar] private Quaternion headRotation;

        [SyncVar] private Vector3 leftHandPosition;
        [SyncVar] private Quaternion leftHandRotation;
        
        [SyncVar] private Vector3 rightHandPosition;
        [SyncVar] private Quaternion rightHandRotation;

        [ClientCallback]
        private void Update()
        {
            if (isLocalPlayer)
            {
                // Instead of sending the local client's headset and hand positions and rotations to the server,
                // we should send the relative positions and rotations to the server
                CmdSyncToServer(
                    LocalTrackedDevice.Instance.CentreAnchorEyeTransform.position,
                    LocalTrackedDevice.Instance.CentreAnchorEyeTransform.rotation,
                    LocalTrackedDevice.Instance.LeftAnchorTransform.position,
                    LocalTrackedDevice.Instance.LeftAnchorTransform.rotation,
                    LocalTrackedDevice.Instance.RightAnchorTransform.position,
                    LocalTrackedDevice.Instance.RightAnchorTransform.rotation);
                UpdateVRObjectsLocally();
            }
            else
                RemoteClientReceiveData();
        }

        /// <summary>
        /// Command that receives the local client's headset and hand positions and rotations and sends it to the server,
        /// the server then updates all the syncvars with the local client's data
        /// </summary>
        [Command]
        private void CmdSyncToServer(
            Vector3 headPosition,
            Quaternion headRotation,
            Vector3 leftHandPosition,
            Quaternion leftHandRotation,
            Vector3 rightHandPosition,
            Quaternion rightHandRotation)
        {
            this.headPosition = headPosition;
            this.headRotation = headRotation;
            headObject.SetPositionAndRotation(headPosition, headRotation);

            this.leftHandPosition = leftHandPosition;
            this.leftHandRotation = leftHandRotation;
            leftHandObject.SetPositionAndRotation(leftHandPosition, leftHandRotation);

            this.rightHandPosition = rightHandPosition;
            this.rightHandRotation = rightHandRotation;
            rightHandObject.SetPositionAndRotation(rightHandPosition, rightHandRotation);
        }

        /// <summary>
        /// Updates the remote client's headset and hand positions and rotations
        /// </summary>
        [ClientCallback]
        private void RemoteClientReceiveData()
        {
            headObject.SetPositionAndRotation(headPosition, headRotation);
            leftHandObject.SetPositionAndRotation(leftHandPosition, leftHandRotation);
            rightHandObject.SetPositionAndRotation(rightHandPosition, rightHandRotation);
        }

        private void UpdateVRObjectsLocally()
        {
            headObject.SetPositionAndRotation(LocalTrackedDevice.Instance.CentreAnchorEyeTransform.position, LocalTrackedDevice.Instance.CentreAnchorEyeTransform.rotation);
            leftHandObject.SetPositionAndRotation(LocalTrackedDevice.Instance.LeftAnchorTransform.position, LocalTrackedDevice.Instance.LeftAnchorTransform.rotation);
            rightHandObject.SetPositionAndRotation(LocalTrackedDevice.Instance.RightAnchorTransform.position, LocalTrackedDevice.Instance.RightAnchorTransform.rotation);
        }
    }
}
