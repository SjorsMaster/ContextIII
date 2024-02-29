using Mirror;
using UnityEngine;

namespace ContextIII
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class ServerManager : NetworkBehaviour
    {
        [SerializeField] private Camera serverCamera;

        [ServerCallback]
        private void Start()
        {
            if (isServer)
                serverCamera.gameObject.SetActive(true);
        }
    }
}
