using Mirror;
using UnityEngine;

namespace ContextIII
{

    // Each local client sends its headset and hand data to the server, which then sends it to all remote clients
    public class ClientVRPositionSync2 : NetworkBehaviour
    {
        [SerializeField] private LocalTrackedDevice localTrackedDevicePrefab;

        [SerializeField] protected Transform headObject;
        [SerializeField] protected Transform leftHandObject;
        [SerializeField] protected Transform rightHandObject;

        [SyncVar] protected Vector3 headPosition;
        [SyncVar] protected Quaternion headRotation;

        [SyncVar] protected Vector3 leftHandPosition;
        [SyncVar] protected Quaternion leftHandRotation;
        
        [SyncVar] protected Vector3 rightHandPosition;
        [SyncVar] protected Quaternion rightHandRotation;

        [ClientCallback]
        protected virtual void Update()
        {
            if (isLocalPlayer)
            {
                LocalTrackedDevice device = LocalTrackedDevice.Instance;
                CmdSyncToServer(
                    device.CentreAnchorEyeRelative.transform.position,
                    device.CentreAnchorEyeRelative.transform.rotation,
                    device.LeftAnchorRelative.transform.position,
                    device.LeftAnchorRelative.transform.rotation,
                    device.RightAnchorRelative.transform.position,
                    device.RightAnchorRelative.transform.rotation);
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
        protected void CmdSyncToServer(
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
        protected virtual void RemoteClientReceiveData()
        {
            headObject.SetPositionAndRotation(headPosition, headRotation);
            leftHandObject.SetPositionAndRotation(leftHandPosition, leftHandRotation);
            rightHandObject.SetPositionAndRotation(rightHandPosition, rightHandRotation);
        }

        protected void UpdateVRObjectsLocally()
        {
            LocalTrackedDevice device = LocalTrackedDevice.Instance;
            headObject.SetPositionAndRotation(device.CentreAnchorEyeRelative.transform.position, device.CentreAnchorEyeRelative.transform.rotation);
            leftHandObject.SetPositionAndRotation(device.LeftAnchorRelative.transform.position, device.LeftAnchorRelative.transform.rotation);
            rightHandObject.SetPositionAndRotation(device.RightAnchorRelative.transform.position, device.RightAnchorRelative.transform.rotation);
        }
    }
}
