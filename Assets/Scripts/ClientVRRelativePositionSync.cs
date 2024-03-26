//using Mirror;
//using UnityEngine;

//namespace ContextIII
//{
//    public class ClientVRRelativePositionSync : ClientVRPositionSync2
//    {
//        private RelativeObject headRelative;
//        private RelativeObject leftHandRelative;
//        private RelativeObject rightHandRelative;

//        [ClientCallback]
//        public override void OnStartAuthority()
//        {
//            base.OnStartAuthority();

//            headRelative = headObject.GetComponent<RelativeObject>();
//            headRelative.enabled = true;
//            leftHandRelative = leftHandObject.GetComponent<RelativeObject>();
//            leftHandRelative.enabled = true;
//            rightHandRelative = rightHandObject.GetComponent<RelativeObject>();
//            rightHandRelative.enabled = true;
//        }

//        protected override void Start()
//        {
//            base.Start();
//            if (!isOwned)
//            {
//                headRelative = headObject.GetComponent<RelativeObject>();
//                headRelative.enabled = true;
//                leftHandRelative = leftHandObject.GetComponent<RelativeObject>();
//                leftHandRelative.enabled = true;
//                rightHandRelative = rightHandObject.GetComponent<RelativeObject>();
//                rightHandRelative.enabled = true;
//            }
//        }

//        protected override void Update()
//        {
//            if (isLocalPlayer)
//            {
//                LocalTrackedDevice device = LocalTrackedDevice.Instance;
//                (Vector3 relativeEyePostion, Vector3 relativeEyeEuler) = device.CentreEyeAnchor.RelativeToSource;
//                (Vector3 relativeLeftPosition, Vector3 relativeLeftEuler) = device.LeftHandAnchor.RelativeToSource;
//                (Vector3 relativeRightPostion, Vector3 relativeRightEuler) = device.RightHandAnchor.RelativeToSource;
//                // Instead of sending the local client's headset and hand positions and rotations to the server,
//                // we should send the relative positions and rotations to the server
//                CmdSyncToServer(
//                    relativeEyePostion,
//                    Quaternion.Euler(device.CentreEyeAnchor.transform.eulerAngles + relativeEyeEuler),
//                    relativeLeftPosition,
//                    Quaternion.Euler(device.LeftHandAnchor.transform.eulerAngles + relativeLeftEuler),
//                    relativeRightPostion,
//                    Quaternion.Euler(device.RightHandAnchor.transform.eulerAngles + relativeRightEuler));
//                //UpdateVRObjectsLocally();
//            }
//            else
//                RemoteClientReceiveData();
//        }

//        [ClientCallback]
//        override protected void RemoteClientReceiveData()
//        {
//            SourceOrigin sourceOrigin = SourceOrigin.TryGetInstance();
//            if (!sourceOrigin)
//                return;

//            headRelative.SetStartPositionAndEuler(headPosition, headRotation.eulerAngles);
//            leftHandRelative.SetStartPositionAndEuler(leftHandPosition, leftHandRotation.eulerAngles);
//            rightHandRelative.SetStartPositionAndEuler(rightHandPosition, rightHandRotation.eulerAngles);

//            sourceOrigin.RecalculateRelativeTransformsToLeftController(RelativeType.Moving_ServerToClient);
//        }
//    }
//}
