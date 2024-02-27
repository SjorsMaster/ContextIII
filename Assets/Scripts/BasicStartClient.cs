using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ContextIII
{
    public class BasicStartClient : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] string networkAddress;

        void Start()
        {
            if (!string.IsNullOrEmpty(networkAddress))
                networkManager.networkAddress = networkAddress;

            networkManager.StartClient();
        }
    }
}
