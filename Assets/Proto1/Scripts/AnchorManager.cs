using SharedSpaces;
using System.Collections;
using UnityEngine;

namespace ContextIII.Proto1
{
    public class AnchorManager : MonoBehaviour
    {
        [SerializeField] private GameObject anchorPrefab;

        private void SpatialAnchorManager_OnDebugMessage(string message)
        {
            VRDebugPanel.Instance.SendDebugMessage(message);
        }

        private void SpatialAnchorSharingManager_OnAnchorShareCompleted(OVRSpatialAnchor[] anchors, OVRSpatialAnchor.OperationResult result)
        {
            string message = result switch
            {
                OVRSpatialAnchor.OperationResult.Success => "Anchors shared successfully",
                OVRSpatialAnchor.OperationResult.Failure => "Failed to share anchors",
                _ => $"Unknown result (failed sharing anchors, result: {result})"
            };

            VRDebugPanel.Instance.SendDebugMessage(message);
        }

        private void OnEnable()
        {
            SpatialAnchorSharingManager.Instance.OnDebugMessage += SpatialAnchorManager_OnDebugMessage;
            SpatialAnchorSharingManager.Instance.OnAnchorShareCompleted += SpatialAnchorSharingManager_OnAnchorShareCompleted;
        }

        private void OnDisable()
        {
            SpatialAnchorSharingManager.Instance.OnDebugMessage -= SpatialAnchorManager_OnDebugMessage;
            SpatialAnchorSharingManager.Instance.OnAnchorShareCompleted -= SpatialAnchorSharingManager_OnAnchorShareCompleted;
        }

        private void Update()
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick) && OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
                StartCoroutine(PlaceAnchor());

            // Load saved anchors from the cloud with left controller's Two button
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                VRDebugPanel.Instance.SendDebugMessage("Loading all anchors from the cloud");
                StartCoroutine(SpatialAnchorSharingManager.Instance.LoadAnchors(anchorPrefab));
            }
        }

        private IEnumerator PlaceAnchor()
        {
            yield return SpatialAnchorManager.Instance.CoCreateSpatialAnchor(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
            yield return SpatialAnchorManager.Instance.CoSaveAnchors(OVRSpace.StorageLocation.Cloud);
            SpatialAnchorSharingManager.Instance.SendAnchorData();
            yield return SpatialAnchorSharingManager.Instance.ShareAnchors();
        }
    }

}