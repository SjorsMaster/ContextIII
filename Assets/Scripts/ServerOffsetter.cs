using ContextIII.NetworkSingletons;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace ContextIII
{
    public partial class ServerOffsetter : NetworkSingleton<ServerOffsetter>
    {
        [SerializeField] private float updateDelay = 0.2f;

        public List<ClientHeadsetData> clientHeadsetData = new();

        private ClientVRPositionSync[] clients;

        //public string debugString;
        private float updateDelayTimer;

        private void Start()
        {
            updateDelayTimer = updateDelay;
        }

        [ServerCallback]
        private void Update()
        {
            if (updateDelayTimer > 0f)
            {
                updateDelayTimer -= Time.deltaTime;
                return;
            }
            else
                updateDelayTimer = updateDelay;

            if (clients == null)
            {
                Debug.Log("No clients found yet.");
                return;
            }

            // Before this was a for loop to do a RPC call foreach client, for each headset data, but an
            // RPC call already sends from the server to every client, so  that seems like a lot of trafic.
            //for (int i = 0; i < clients.Length; i++)
            //    //clients[i].RpcDebug(debugString);
            for (int j = 0; j < clientHeadsetData.Count; j++)
                RpcSetClient(clientHeadsetData[j].netId,
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
        }

        [ClientRpc]
        public void RpcSetClient(
            uint id,
            Vector3 head,
            Vector3 leftHand,
            Vector3 rightHand,
            Vector3 handUp,
            Vector3 handForward,
            Vector3 leftHandUp,
            Vector3 leftHandForward,
            Vector3 rightHandUp,
            Vector3 rightHandForward,
            bool isActor)
        {
            Debug.Log("This was called on a client");

            List<ClientVRPositionSync> clients = GameData.Clients;
            CalibrationSource source = CalibrationSource.Instance;

            foreach (ClientVRPositionSync client in clients)
            {
                if (client != this && id == client.netId) // Only correct other headsets.
                {
                    Transform t = source.transform;
                    client.head.position = t.TransformPoint(head * (1 / t.localScale.x));
                    client.head.LookAt(client.head.position + t.TransformDirection(handForward), t.TransformDirection(handUp));
                    client.leftHand.position = t.TransformPoint(leftHand * (1 / t.localScale.x));
                    client.leftHand.LookAt(client.leftHand.position + t.TransformDirection(leftHandForward), t.TransformDirection(leftHandUp));
                    client.rightHand.position = t.TransformPoint(rightHand * (1 / t.localScale.x));
                    client.rightHand.LookAt(client.rightHand.position + t.TransformDirection(rightHandForward), t.TransformDirection(rightHandUp));

                    if (isActor) // && AshantiManController.instance)
                    {
                        // TODO: Abstract this away from core networking
                        /*
                           Vector3 ashantiTarget = new Vector3(clients[i].head.position.x, t.position.y, clients[i].head.position.z);
                           AshantiManController.instance.transform.position = ashantiTarget;
                        */

                        client.head.gameObject.SetActive(false);
                        client.LeftHandObject.SetActive(false);
                        client.RightHandObject.SetActive(false);
                    }
                }
            }
        }

        [ServerCallback]
        public void UpdateClientPositions(uint id, Vector3 headOffset, Vector3 leftHandOffset, Vector3 rightHandOffset, Vector3 headUp, Vector3 headForward, Vector3 leftHandUp, Vector3 leftHandForward, Vector3 rightHandUp, Vector3 rightHandForward, bool isActor)
        {
            for (int i = 0; i < clientHeadsetData.Count; i++)
                if (clientHeadsetData[i].netId == id)
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

        [Server]
        public void AddClient(ClientVRPositionSync client)
        {
            Debug.Log("Did someone call the AddClient function?");
            GameData.Clients.Add(client);
        }
    }
}