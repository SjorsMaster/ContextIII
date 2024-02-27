using UnityEngine;

namespace ContextIII
{
    [System.Serializable]
    public struct ClientHeadsetData
    {
        public uint netId;
        public Vector3 head;
        public Vector3 leftHand;
        public Vector3 rightHand;
        public Vector3 headUp;
        public Vector3 headForward;
        public Vector3 leftHandUp;
        public Vector3 leftHandForward;
        public Vector3 rightHandUp;
        public Vector3 rightHandForward;
        public bool isActor;
    }
}