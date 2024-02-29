using Mirror;
using UnityEngine;

namespace ContextIII
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class ServerManager : NetworkBehaviour
    {
        [SerializeField] private Camera serverCamera;

        private void Start()
        {
            if (isServer)
                serverCamera.gameObject.SetActive(true);
            else
                serverCamera.gameObject.SetActive(false);
        }
    }
}
