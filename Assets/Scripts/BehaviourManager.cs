using Mirror;
using UnityEngine;

namespace ContextIII
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class BehaviourManager : NetworkBehaviour
    {
        [SerializeField] private Behaviour[] behavioursToEnableOnServer;
        [SerializeField] private Behaviour[] behavioursToDisableOnServer;
        [SerializeField] private Behaviour[] behavioursToEnableOnClient;
        [SerializeField] private Behaviour[] behavioursToDisableOnClient;

        private void Start()
        {
            if (isServer)
            {
                foreach (var behaviour in behavioursToEnableOnServer)
                    behaviour.enabled = true;
                foreach (var behaviour in behavioursToDisableOnServer)
                    behaviour.enabled = false;
            }
            else
            {
                foreach (var behaviour in behavioursToEnableOnClient)
                    behaviour.enabled = true;
                foreach (var behaviour in behavioursToDisableOnClient)
                    behaviour.enabled = false;
            }
        }
    }
}
