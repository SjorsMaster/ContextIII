using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ContextIII
{
    //This script also handles most if not all clientside logic such as RPC's
    [RequireComponent(typeof(ActorHandler))]
    public class ClientVRPositionSync : NetworkBehaviour
    {
        [field: SerializeField] public GameObject LeftHandObject { get; private set; }
        [field: SerializeField] public GameObject RightHandObject { get; private set; }

        [SerializeField] private LocalTrackedDevice trackedDevicePrefab;

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

        public Transform head;
        public Transform leftHand;
        public Transform rightHand;

        private LocalTrackedDevice trackedDevice;

        private void Awake()
        {
            actorHandler = GetComponent<ActorHandler>();
        }

        [ClientCallback]
        private void Start()
        {
            GameData.Clients.Add(this);

            if (debugBallsToggle)
            {
                debugRendererToggle = debugBallsToggle.action;
                debugRendererToggle.started += DebugRenderToggle;
            }

            if (isLocalPlayer)
            {
                trackedDevice = Instantiate(trackedDevicePrefab, transform);
                LeftHandObject.SetActive(false);
                RightHandObject.SetActive(false);
                CmdSendAddRequest();
            }

            ServerOffsetter.Instance.clientHeadsetData.Add(new ClientHeadsetData
            {
                netId = netId,
                isActor = actorHandler.IsActor
            });
        }

        [ClientCallback]
        private void LateUpdate()
        {
            if (!isLocalPlayer)
                return;

            //Synchronize player transforms with OVR Rig transforms
            Synchronize(head, trackedDevice.CentreAnchorEyeTransform);                // Head
            Synchronize(leftHand, trackedDevice.LeftAnchorTransform);        // Left Hand
            Synchronize(rightHand, trackedDevice.RightAnchorTransform);      // Right Hand

            CalibrationSource source = CalibrationSource.Instance;
            if (source)
            {
                Transform t = source.transform;
                headOffset = t.InverseTransformPoint(trackedDevice.CentreAnchorEyeTransform.position);
                headOffset.Scale(t.localScale);
                headUp = t.InverseTransformDirection(trackedDevice.CentreAnchorEyeTransform.up);
                headForward = t.InverseTransformDirection(trackedDevice.CentreAnchorEyeTransform.forward);

                leftHandOffset = t.InverseTransformPoint(trackedDevice.LeftAnchorTransform.position);
                leftHandOffset.Scale(t.localScale);
                leftHandUp = t.InverseTransformDirection(trackedDevice.LeftAnchorTransform.up);
                leftHandForward = t.InverseTransformDirection(trackedDevice.LeftAnchorTransform.forward);

                rightHandOffset = t.InverseTransformPoint(trackedDevice.RightAnchorTransform.position);
                rightHandOffset.Scale(t.localScale);
                rightHandUp = t.InverseTransformDirection(trackedDevice.RightAnchorTransform.up);
                rightHandForward = t.InverseTransformDirection(trackedDevice.RightAnchorTransform.forward);
            }

            CmdAddServersideOffset();

            if (actorHandler.IsActor)
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

            ServerOffsetter.Instance.UpdateClientPositions(
                netId,
                headOffset,
                leftHandOffset,
                rightHandOffset,
                headUp,
                headForward,
                leftHandUp,
                leftHandForward,
                rightHandUp,
                rightHandForward,
                actorHandler.IsActor);
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
        private void CmdSendAddRequest()
        {
            ServerOffsetter.Instance.AddClient(this);
        }
    }
}