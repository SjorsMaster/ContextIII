using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace ContextIII
{
    public class ServerOffsetter : NetworkBehaviour
    {
        public static ServerOffsetter instance;

        [System.Serializable]
        public struct ClientHeadsetData
        {
            public uint netId;
            public Vector3 head, leftHand, rightHand, headUp, headForward, leftHandUp, leftHandForward, rightHandUp, rightHandForward;
            public bool isActor;
        }

        public List<ClientHeadsetData> clientHeadsetData = new();

        //public string debugString;
        [SerializeField]
        private float m_UpdateDelay = 0.2f;
        private float m_UpdateDelayTimer;

        private ClientVRPositionSync[] clients;

        public float UpdateDelay { get => m_UpdateDelay; }

        private void Start()
        {
            //Create Singleton
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            m_UpdateDelayTimer = UpdateDelay;
        }

        [ServerCallback]
        public void UpdateClientPositions(uint id, Vector3 headOffset, Vector3 leftHandOffset, Vector3 rightHandOffset, Vector3 headUp, Vector3 headForward, Vector3 leftHandUp, Vector3 leftHandForward, Vector3 rightHandUp, Vector3 rightHandForward, bool isActor)
        {
            for (int i = 0; i < clientHeadsetData.Count; i++)
            {
                if (clientHeadsetData[i].netId == id)
                {
                    //Debug.Log($"Client {id} sends headoffset {headOffset}, leftHandOffset {leftHandOffset} and rightHandOffset {rightHandOffset}. Actor flag is {isActor}.");
                    clientHeadsetData[i] = new ClientHeadsetData
                    {
                        netId = id,
                        head = headOffset,
                        leftHand = leftHandOffset,
                        rightHand = rightHandOffset,
                        headUp = headUp,
                        headForward = headForward,
                        isActor = isActor,
                        leftHandUp = leftHandUp,
                        leftHandForward = leftHandForward,
                        rightHandUp = rightHandUp,
                        rightHandForward = rightHandForward
                    };
                }
            }
        }

        [ServerCallback]
        private void Update()
        {
            if (m_UpdateDelayTimer > 0f)
            {
                m_UpdateDelayTimer -= Time.deltaTime;
                return;
            }
            else
            {
                m_UpdateDelayTimer = UpdateDelay;
            }

            if (clients == null)
            {
                Debug.Log("No clients found yet.");
                return;
            }

            for (int i = 0; i < clients.Length; i++)
            {
                //clients[i].RpcDebug(debugString);
                for (int j = 0; j < clientHeadsetData.Count; j++)
                {
                    clients[i].RpcSetClient(clientHeadsetData[j].netId,
                        clientHeadsetData[j].head,
                        clientHeadsetData[j].leftHand,
                        clientHeadsetData[j].rightHand,
                        clientHeadsetData[j].headUp,
                        clientHeadsetData[j].headForward,
                        clientHeadsetData[j].leftHandUp,
                        clientHeadsetData[j].leftHandForward,
                        clientHeadsetData[j].rightHandUp,
                        clientHeadsetData[j].rightHandForward,
                        clientHeadsetData[j].isActor);

                    // TODO: Abstract this away from core networking
                    /*
                    if (clientHeadsetData[j].isActor)
                    {
                        AshantiManController.instance.transform.position = new Vector3(clients[i].head.position.x, 0, clients[i].head.position.z);
                    }
                    */
                }

            }

        }

        public void DetectClients()
        {
            Debug.Log("Did someone call DetectClients()?");
            clients = FindObjectsOfType<ClientVRPositionSync>();
        }

    }
}