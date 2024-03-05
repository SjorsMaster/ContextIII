using Mirror;
using UnityEngine;

namespace ContextIII
{
    // Each local client sends its headset and hand data to the server, which then sends it to all remote clients
    public class ClientVRPositionSync2 : NetworkBehaviour
    {
        [SerializeField] private TrackedDevice trackedDevicePrefab;

        [SerializeField] private Transform headObject;
        [SerializeField] private Transform leftHandObject;
        [SerializeField] private Transform rightHandObject;

        [SyncVar] private Vector3 headPosition;
        [SyncVar] private Quaternion headRotation;

        [SyncVar] private Vector3 leftHandPosition;
        [SyncVar] private Quaternion leftHandRotation;
        
        [SyncVar] private Vector3 rightHandPosition;
        [SyncVar] private Quaternion rightHandRotation;

        private TrackedDevice trackedDevice;

        private void Start()
        {
            if (isLocalPlayer)
                trackedDevice = Instantiate(trackedDevicePrefab, transform);
        }

        [ClientCallback]
        private void Update()
        {
            if (isLocalPlayer)
            {
                CmdSyncToServer(
                    trackedDevice.TrackedHeadTransform.position,
                    trackedDevice.TrackedHeadTransform.rotation,
                    trackedDevice.TrackedLeftHandTransform.position,
                    trackedDevice.TrackedLeftHandTransform.rotation,
                    trackedDevice.TrackedRightHandTransform.position,
                    trackedDevice.TrackedRightHandTransform.rotation);
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
            headObject.SetPositionAndRotation(trackedDevice.TrackedHeadTransform.position, trackedDevice.TrackedHeadTransform.rotation);
            leftHandObject.SetPositionAndRotation(trackedDevice.TrackedLeftHandTransform.position, trackedDevice.TrackedLeftHandTransform.rotation);
            rightHandObject.SetPositionAndRotation(trackedDevice.TrackedRightHandTransform.position, trackedDevice.TrackedRightHandTransform.rotation);
        }
    }
}
