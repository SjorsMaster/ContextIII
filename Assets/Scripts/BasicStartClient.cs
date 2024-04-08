using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

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
