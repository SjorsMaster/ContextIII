using UnityEngine;
using Mirror;

namespace TU.PerfNet
{
    public class AutoConnector : MonoBehaviour
    {
        public enum AutoConnectionType
        {
            Host = 0,
            ClientOnly = 1,
            ServerOnly = 2
        }

        public AutoConnectionType autoConnectionType = 0;
        public string forcedIP = "localhost";
        //public AnimationCurve sendRatePerConnections;

        private NetworkManager networkManager;

        [SerializeField]
        private ServerOffsetter m_Offsetter;

        private void Start()
        {
            networkManager = GetComponent<NetworkManager>();

            networkManager.networkAddress = forcedIP;

            switch (autoConnectionType)
            {
                case AutoConnectionType.Host:
                    networkManager.StartHost();
                    break;
                case AutoConnectionType.ClientOnly:
                    networkManager.StartClient();
                    break;
                case AutoConnectionType.ServerOnly:
                    networkManager.StartServer();
                    break;
                default:
                    Debug.LogWarning("Could not start Mirror connection!");
                    break;
            }
        }

        /*private void LateUpdate()
        {
            if (networkManager)
            {
                networkManager.sendRate = (int)sendRatePerConnections.Evaluate(Mathf.Clamp(m_Offsetter.clientHeadsetData.Count, 0, networkManager.maxConnections));
            }
        }*/
    }
}