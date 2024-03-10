using System.Collections.Generic;
using UnityEngine;

namespace ContextIII
{
    public class RelativeObjectsManager : Singleton<RelativeObjectsManager>
    {
        [SerializeField] private bool trackXPosition = true;
        [SerializeField] private bool trackYPosition = true;
        [SerializeField] private bool trackZPosition = true;

        [SerializeField] private bool trackXRotation = false;
        [SerializeField] private bool trackYRotation = true;
        [SerializeField] private bool trackZRotation = false;

        private readonly List<RelativeObject> relativeObjects = new();

        private Vector3 StartPosition;
        private Vector3 StartEulers;

        private void Start()
        {
            StartPosition = transform.position;
            StartEulers = transform.eulerAngles;
        }

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                RecalculateAllRelativeTransforms();
        }

        public void AddRelativeObject(RelativeObject relativeObject)
        {
            if (!relativeObjects.Contains(relativeObject))
                relativeObjects.Add(relativeObject);
        }

        public void RemoveRelativeObject(RelativeObject relativeObject)
        {
            if (relativeObjects.Contains(relativeObject))
                relativeObjects.Remove(relativeObject);
        }

        public void RecalculateAllRelativeTransforms()
        {
            LocalTrackedDevice localTrackedDevice = LocalTrackedDevice.Instance;

            Transform leftController = localTrackedDevice.LeftAnchorTransform;

            Vector3 positionStartToLeftController = leftController.position - StartPosition;

            if (!trackXPosition)
                positionStartToLeftController.x = 0;

            if (!trackYPosition)
                positionStartToLeftController.y = 0;

            if (!trackZPosition)
                positionStartToLeftController.z = 0;

            Vector3 eulerFromStartToLeftController = leftController.eulerAngles - StartEulers;
            
            if (!trackXRotation)
                eulerFromStartToLeftController.x = 0;

            if (!trackYRotation)
                eulerFromStartToLeftController.y = 0;

            if (!trackZRotation)
                eulerFromStartToLeftController.z = 0;

            transform.SetLocalPositionAndRotation(leftController.position, leftController.rotation);

            foreach (RelativeObject relativeObject in relativeObjects)
            {
                Vector3 positionRelativeStartToStart = StartPosition - relativeObject.StartPosition;
                Vector3 positionStartToRelativeStartRotated = Quaternion.Euler(eulerFromStartToLeftController) * -positionRelativeStartToStart;

                Vector3 result =
                    relativeObject.StartPosition
                    + positionRelativeStartToStart
                    + positionStartToLeftController
                    + positionStartToRelativeStartRotated;
                relativeObject.transform.position = result;
                relativeObject.transform.eulerAngles = relativeObject.StartEuler + eulerFromStartToLeftController;
            }
        }
    }
}