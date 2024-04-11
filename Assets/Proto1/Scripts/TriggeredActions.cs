using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class TriggeredActions : NetworkBehaviour
{
    public UnityEvent entryBehaviour, exitBehaviour;

    [ClientCallback]
    public void OnTriggerEnter(Collider other)
    {
        CmdTriggerEnter();
    }

    [ClientCallback]
    public void OnTriggerExit(Collider other)
    {
        CmdTriggerExit();
    }

    [Command(requiresAuthority = false)]
    public void CmdTriggerEnter()
    {
        RpcTriggerEnter();
    }

    [ClientRpc]
    public void RpcTriggerEnter()
    {
        entryBehaviour.Invoke();
    }

    [Command(requiresAuthority = false)]
    public void CmdTriggerExit()
    {
        RpcTriggerExit();
    }

    [ClientRpc]
    public void RpcTriggerExit()
    {
        exitBehaviour.Invoke();
    }
}