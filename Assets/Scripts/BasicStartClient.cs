using Mirror;
using UnityEngine;

namespace ContextIII
{
    public class BasicStartClient : MonoBehaviour
    {
        [SerializeField] private bool autoConnectOnStartup;
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] string networkAddress;

        private void Start()
        {
            if (autoConnectOnStartup)
                StartClient();
        }

        public void StartClient()
        {
            networkManager.networkAddress = networkAddress;
            networkManager.StartClient();
        }
    }
}
