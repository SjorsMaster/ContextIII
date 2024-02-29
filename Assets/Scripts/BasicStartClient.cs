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
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            inputField.text = networkAddress;

            if (autoConnectOnStartup)
                StartClient();
        }

        public void StartClient()
        {
            if (!string.IsNullOrEmpty(networkAddress))
                networkManager.networkAddress = networkAddress;

            networkManager.StartClient();
        }
    }
}
