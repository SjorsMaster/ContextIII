using UnityEngine;

namespace PortalsVR
{
    public delegate void WorldEvent(string world);
    public class PortalTraveller : MonoBehaviour
    {
        public event WorldEvent onWorldChanged;

        #region Fields
        [SerializeField] private Transform target;
        // Set this to true for objects that need to travel between worlds
        public bool migrateUponTeleport = false;
        // Set this to false for non-player travellers
        public bool deformPortal = true;
        public bool isPlayer = true;
        #endregion

        #region Properties
        public Transform Target => target;

        public Vector3 PreviousOffsetFromPortal { get; set; }
        public bool InPortal { get; set; }
        #endregion

        //[HideInInspector]
        public string activeWorld = "World 1";

        public Eye[] eyes;
        Rigidbody rBody;

        private void Awake()
        {
            if (target == null) target = transform;

            rBody = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            onWorldChanged?.Invoke(activeWorld);
        }

        #region Methods
        public virtual void Teleport(Portal fromPortal, Portal toPortal, Vector3 pos, Quaternion m)
        {
            // It should already be added to the other worlds object list
            //  This is handled in the Portal's OnTriggerExit
            if (migrateUponTeleport)
                gameObject.transform.parent = toPortal.parentWorld.transform;

            transform.position += pos - target.position;

            float relativeScale = toPortal.transform.localScale.magnitude / fromPortal.transform.localScale.magnitude;
            transform.localScale *= relativeScale;

            // Transform the velocities to keep the momentum correct
            if (rBody && !rBody.isKinematic)
            {
                rBody.velocity = toPortal.transform.TransformDirection(fromPortal.transform.InverseTransformDirection(rBody.velocity));
                rBody.angularVelocity = toPortal.transform.TransformDirection(fromPortal.transform.InverseTransformDirection(rBody.angularVelocity));
            }

            Physics.SyncTransforms();

            activeWorld = toPortal.parentWorld.name;
            onWorldChanged?.Invoke(activeWorld);
            foreach (Eye eye in eyes)
            {
                eye.activeWorld = activeWorld;
            }
        }

        public void ForcedTeleport(string targetWorld)
        {
            activeWorld = targetWorld;
            onWorldChanged?.Invoke(activeWorld);
            foreach (Eye eye in eyes)
            {
                eye.activeWorld = activeWorld;
            }
        }
        #endregion

        private void LateUpdate()
        {
            if (isPlayer)
            {
                Shader.SetGlobalVector("_CenterEyePosition", transform.position);
            }
        }
    }
}