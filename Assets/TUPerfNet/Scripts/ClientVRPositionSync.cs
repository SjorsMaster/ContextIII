using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TU.PerfNet
{
    //This script also handles most if not all clientside logic such as RPC's.
    public class ClientVRPositionSync : NetworkBehaviour
    {
        public TextMeshPro debugText;
        public ActorHandler actorHandler;

        public InputActionReference debugBallsToggle; // TODO: Remove debug stuff

        public Transform head, leftHand, rightHand;

        [SerializeField]
        private GameObject leftHandObject, rightHandObject;

        private CalibrationSource source;

        private InputAction debugRendererToggle;

        [SyncVar]
        [SerializeField]
        private Vector3 headOffset, leftHandOffset, rightHandOffset, headUp, headForward, leftHandUp, leftHandForward, rightHandUp, rightHandForward;

        private void Start()
        {
            actorHandler = GetComponent<ActorHandler>();

            if (debugBallsToggle)
            {
                debugRendererToggle = debugBallsToggle.action;
                debugRendererToggle.started += DebugRenderToggle;
            }

            // Should probably just find a single object for this, and if nothing is found, throw an error?
            //  It seems really wasteful to parse a bunch of objects to find 1 thing that is always the same.
            CalibrationSource source = FindObjectOfType<CalibrationSource>();
            if (!source)
            {
                Debug.LogError("Add a CalibrationSource script to an object at the center of your virtual scene (for instance, the floor).");
            }
            else
            {
                if (isLocalPlayer)
                {
                    leftHandObject.SetActive(false);
                    rightHandObject.SetActive(false);
                    CmdSendDetectRequest();
                }

                ServerOffsetter.instance.clientHeadsetData.Add(new ServerOffsetter.ClientHeadsetData { netId = this.netId, isActor = actorHandler.isActor });
            }
        }

        private void LateUpdate()
        {
            LocalPlayerController control = LocalPlayerController.self;
            if (control && isLocalPlayer)
            {
                //Synchronize player transforms with OVR Rig transforms

                Synchronize(head, control.trackedHeadTransform);                //Head
                Synchronize(leftHand, control.trackedLeftHandTransform);        //Left Hand
                Synchronize(rightHand, control.trackedRightHandTransform);      //Right Hand

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
        }

        private void DebugRenderToggle(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer)
            {
                return;
            }

            ClientVRPositionSync[] allClients = FindObjectsOfType<ClientVRPositionSync>();

            foreach (var client in allClients)
            {
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = !renderer.enabled;
                }
            }

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
        public void RpcSetClient(uint id, Vector3 h, Vector3 lH, Vector3 rH, Vector3 hU, Vector3 hF, Vector3 lHU, Vector3 lHF, Vector3 rHU, Vector3 rHF, bool isActor)
        {
            ClientVRPositionSync[] clients = FindObjectsOfType<ClientVRPositionSync>();

            // Calibration source might be unset here. Reassigning.
            if (!source) source = FindObjectOfType<CalibrationSource>();

            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i] != this && id == clients[i].netId) //Only correct other headsets.
                {
                    if ( source )
                    {
                        Transform t = source.transform;
                        clients[i].head.position = t.TransformPoint(h * (1 / t.localScale.x));
                        clients[i].head.LookAt(clients[i].head.position + t.TransformDirection(hF), t.TransformDirection(hU));
                        clients[i].leftHand.position = t.TransformPoint(lH * (1 / t.localScale.x));
                        clients[i].leftHand.LookAt(clients[i].leftHand.position + t.TransformDirection(lHF), t.TransformDirection(lHU));
                        clients[i].rightHand.position = t.TransformPoint(rH * (1 / t.localScale.x));
                        clients[i].rightHand.LookAt(clients[i].rightHand.position + t.TransformDirection(rHF), t.TransformDirection(rHU));
                    }
                    if (isActor) // && AshantiManController.instance)
                    {
                        // TODO: Abstract this away from core networking
                        /*
                           Vector3 ashantiTarget = new Vector3(clients[i].head.position.x, t.position.y, clients[i].head.position.z);
                           AshantiManController.instance.transform.position = ashantiTarget;
                        */

                        clients[i].head.gameObject.SetActive(false);
                        clients[i].leftHandObject.gameObject.SetActive(false);
                        clients[i].rightHandObject.gameObject.SetActive(false);
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

            if ( source )
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