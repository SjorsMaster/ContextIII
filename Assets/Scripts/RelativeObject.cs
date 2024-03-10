using UnityEngine;

namespace ContextIII
{
    public class RelativeObject : MonoBehaviour
    {
        public Vector3 StartPosition { get; private set; }
        public Vector3 StartEuler { get; private set; }

        private void OnEnable()
        {
            StartPosition = transform.position;
            StartEuler = transform.eulerAngles;

            RelativeObjectsManager relativeObjectsManager = RelativeObjectsManager.Instance;
            relativeObjectsManager.AddRelativeObject(this);
        }

        private void OnDisable()
        {
            transform.position = StartPosition;
            transform.eulerAngles = StartEuler;

            RelativeObjectsManager relativeObjectsManager = RelativeObjectsManager.Instance;
            relativeObjectsManager.RemoveRelativeObject(this);
        }
    }
}