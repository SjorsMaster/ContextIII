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
        [SerializeField] private TMP_InputField ipInputField;

        private void Start()
        {
            if (autoConnectOnStartup)
                StartClient();
        }

        public void StartClient()
        {
            if (!string.IsNullOrEmpty(networkAddress))
                networkManager.networkAddress = networkAddress;
            else
                networkManager.networkAddress = "localhost";

            if (ipInputField != null && !string.IsNullOrEmpty(ipInputField.text))
                networkManager.networkAddress = ipInputField.text;

            networkManager.StartClient();
        }
    }
}
