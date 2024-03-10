using System.Collections.Generic;
using UnityEngine;

namespace ContextIII
{
    public class RelativeObjectsManager : Singleton<RelativeObjectsManager>
    {
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
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
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
            Vector3 eulerFromStartToLeftController = leftController.eulerAngles - StartEulers;

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