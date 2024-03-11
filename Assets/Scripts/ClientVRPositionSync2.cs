using Mirror;
using UnityEngine;

namespace ContextIII
{
    public class ClientVRRelativePositionSync : ClientVRPositionSync2
    {
        protected override void Update()
        {
            if (isLocalPlayer)
            {
                LocalTrackedDevice device = LocalTrackedDevice.Instance;
                (Vector3 relativeEyePostion, Vector3 relativeEyeEuler) = device.CentreAnchorEyeRelative.RelativeToSource;
                (Vector3 relativeLeftPosition, Vector3 relativeLeftEuler) = device.LeftAnchorRelative.RelativeToSource;
                (Vector3 relativeRightPostion, Vector3 relativeRightEuler) = device.RightAnchorRelative.RelativeToSource;
                // Instead of sending the local client's headset and hand positions and rotations to the server,
                // we should send the relative positions and rotations to the server
                CmdSyncToServer(
                    relativeEyePostion,
                    Quaternion.Euler(relativeEyeEuler),
                    relativeLeftPosition,
                    Quaternion.Euler(relativeLeftEuler),
                    relativeRightPostion,
                    Quaternion.Euler(relativeRightEuler));
                UpdateVRObjectsLocally();
            }
            else
                RemoteClientReceiveData();
        }
    }

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
        protected void RemoteClientReceiveData()
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
