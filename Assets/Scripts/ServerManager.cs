using Mirror;
using UnityEngine;

namespace ContextIII
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class ServerManager : NetworkBehaviour
    {
        [SerializeField] private Camera serverCamera;
        [SerializeField] private SourceOrigin sourceOrigin;

        public override void OnStartServer()
        {
            base.OnStartServer();
            serverCamera.gameObject.SetActive(true);
            sourceOrigin.enabled = false;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            serverCamera.gameObject.SetActive(false);
            sourceOrigin.enabled = true;
        }
    }
}
