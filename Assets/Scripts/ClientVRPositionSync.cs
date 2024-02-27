using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ContextIII
{
    //This script also handles most if not all clientside logic such as RPC's.
    [RequireComponent(typeof(ActorHandler))]
    public class ClientVRPositionSync : NetworkBehaviour
    {
        [SerializeField] private GameObject leftHandObject, rightHandObject;

        // find out why this is serialized
        [SerializeField, SyncVar] private Vector3 headOffset;
        [SerializeField, SyncVar] private Vector3 leftHandOffset;
        [SerializeField, SyncVar] private Vector3 rightHandOffset;
        [SerializeField, SyncVar] private Vector3 headUp;
        [SerializeField, SyncVar] private Vector3 headForward;
        [SerializeField, SyncVar] private Vector3 leftHandUp;
        [SerializeField, SyncVar] private Vector3 leftHandForward;
        [SerializeField, SyncVar] private Vector3 rightHandUp;
        [SerializeField, SyncVar] private Vector3 rightHandForward;

        private ActorHandler actorHandler;

        private InputAction debugRendererToggle;

        public InputActionReference debugBallsToggle; // TODO: Remove debug stuff

        public TextMeshPro debugText;

        public Transform head, leftHand, rightHand;

        // TODO:
        // - Add clients to GameData list on server side

        private void Start()
        {
            actorHandler = GetComponent<ActorHandler>();

            if (debugBallsToggle)
            {
                debugRendererToggle = debugBallsToggle.action;
                debugRendererToggle.started += DebugRenderToggle;
            }

            if (isLocalPlayer)
            {
                leftHandObject.SetActive(false);
                rightHandObject.SetActive(false);
                CmdSendDetectRequest();
            }

            ServerOffsetter.instance.clientHeadsetData.Add(new ServerOffsetter.ClientHeadsetData { netId = this.netId, isActor = actorHandler.isActor });
        }

        private void LateUpdate()
        {
            LocalPlayerController control = LocalPlayerController.self;
            if (!control || !isLocalPlayer)
                return;

            //Synchronize player transforms with OVR Rig transforms
            Synchronize(head, control.trackedHeadTransform);                // Head
            Synchronize(leftHand, control.trackedLeftHandTransform);        // Left Hand
            Synchronize(rightHand, control.trackedRightHandTransform);      // Right Hand

            CalibrationSource source = CalibrationSource.Instance;
            if (source)
            {
                Transform t = source.transform;
                headOffset = t.InverseTransformPoint(control.trackedHeadTransform.position);
                headOffset.Scale(t.localScale);
                headUp = t.InverseTransformDirection(control.trackedHeadTransform.up);
                headForward = t.InverseTransformDirection(control.trackedHeadTransform.forward);

                leftHandOffset = t.InverseTransformPoint(control.trackedLeftHandTransform.position);
                leftHandOffset.Scale(t.localScale);
                leftHandUp = t.InverseTransformDirection(control.trackedLeftHandTransform.up);
                leftHandForward = t.InverseTransformDirection(control.trackedLeftHandTransform.forward);

                rightHandOffset = t.InverseTransformPoint(control.trackedRightHandTransform.position);
                rightHandOffset.Scale(t.localScale);
                rightHandUp = t.InverseTransformDirection(control.trackedRightHandTransform.up);
                rightHandForward = t.InverseTransformDirection(control.trackedRightHandTransform.forward);
            }

            CmdAddServersideOffset();

            if (actorHandler.isActor)
            {
                // TODO: Abstract this using an interface or something...
                // AshantiManController.instance.gameObject.SetActive(false);
            }
        }

        private void DebugRenderToggle(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer)
                return;

            ClientVRPositionSync[] allClients = FindObjectsOfType<ClientVRPositionSync>();

            foreach (var client in allClients)
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                    renderer.enabled = !renderer.enabled;
        }

        [Client]
        private void Synchronize(Transform target, Transform control)
        {
            target.SetPositionAndRotation(control.position, control.rotation);
        }

        [Command(requiresAuthority = false)]
        private void CmdAddServersideOffset()
        {
            head.position = headOffset;
            head.up = headUp;
            head.forward = headForward;

            leftHand.position = leftHandOffset;
            leftHand.up = leftHandUp;
            leftHand.forward = leftHandForward;

            rightHand.position = rightHandOffset;
            rightHand.up = rightHandUp;
            rightHand.forward = rightHandForward;

            ServerOffsetter.instance.UpdateClientPositions(netId, headOffset, leftHandOffset, rightHandOffset, headUp, headForward, leftHandUp, leftHandForward, rightHandUp, rightHandForward, actorHandler.isActor);
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
            List<ClientVRPositionSync> clients = GameData.Clients;
            CalibrationSource source = CalibrationSource.Instance;

            foreach (ClientVRPositionSync client in clients)
            {
                if (client != this && id == client.netId) // Only correct other headsets.
                {
                    if (source)
                    {
                        Transform t = source.transform;
                        client.head.position = t.TransformPoint(head * (1 / t.localScale.x));
                        client.head.LookAt(client.head.position + t.TransformDirection(handForward), t.TransformDirection(handUp));
                        client.leftHand.position = t.TransformPoint(leftHand * (1 / t.localScale.x));
                        client.leftHand.LookAt(client.leftHand.position + t.TransformDirection(leftHandForward), t.TransformDirection(leftHandUp));
                        client.rightHand.position = t.TransformPoint(rightHand * (1 / t.localScale.x));
                        client.rightHand.LookAt(client.rightHand.position + t.TransformDirection(rightHandForward), t.TransformDirection(rightHandUp));
                    }

                    if (isActor) // && AshantiManController.instance)
                    {
                        // TODO: Abstract this away from core networking
                        /*
                           Vector3 ashantiTarget = new Vector3(clients[i].head.position.x, t.position.y, clients[i].head.position.z);
                           AshantiManController.instance.transform.position = ashantiTarget;
                        */

                        client.head.gameObject.SetActive(false);
                        client.leftHandObject.SetActive(false);
                        client.rightHandObject.SetActive(false);
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcSetLocalScene(int buildIndex, LoadSceneMode loadSceneMode)
        {
            if (isLocalPlayer)
            {
                StartCoroutine(DoSetLocalScene(buildIndex, loadSceneMode));
            }
        }

        [ClientRpc]
        public void RpcUnloadLocalScene(int buildIndex)
        {
            if (isLocalPlayer)
            {
                StartCoroutine(DoUnloadLocalScene(buildIndex));
            }
        }

        public IEnumerator DoSetLocalScene(int buildIndex, LoadSceneMode loadSceneMode)
        {

            AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex, loadSceneMode);

            while (!op.isDone)
            {
                yield return null; //Keep moving on one frame as long as the scene needs to be loaded.
            }

            //Scene has been loaded. Redo scene calibration logic.
            CalibrationSource source = FindObjectOfType<CalibrationSource>();
            if (!source)
            {
                Debug.LogError($"Add a CalibrationSource script to an object at the center of your virtual scene (for instance, the floor). Scene: {SceneManager.GetActiveScene().name}");
            }

            SceneParent newScene = FindObjectOfType<SceneParent>();

            if (source)
                newScene.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);

            newScene.Apply();
        }

        public IEnumerator DoUnloadLocalScene(int buildIndex)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(buildIndex);

            while (!op.isDone)
            {
                yield return null; //Skipping frames until the scene has been unloaded.
            }

            yield break;
        }

        [ClientRpc]
        public void RpcDebug(string debugString)
        {
            if (debugText == null)
            {
                debugText = GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>();
            }
            debugText.text = debugString;
        }

        [Command]
        private void CmdSendDetectRequest()
        {
            ServerOffsetter.instance.DetectClients();
        }
    }
}