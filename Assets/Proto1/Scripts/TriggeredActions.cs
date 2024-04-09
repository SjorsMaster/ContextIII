using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class TriggeredActions : NetworkBehaviour
{
    public UnityEvent entryBehaviour, exitBehaviour;

    [ServerCallback]
    public void OnTriggerEnter(Collider other)
    {
        RpcTriggerEnter();
    }

    [ServerCallback]
    public void OnTriggerExit(Collider other)
    {
        RpcTriggerExit();
    }

    [ClientRpc]
    public void RpcTriggerEnter()
    {
        entryBehaviour.Invoke();
    }

    [ClientRpc]
    public void RpcTriggerExit()
    {
        exitBehaviour.Invoke();
    }
}
