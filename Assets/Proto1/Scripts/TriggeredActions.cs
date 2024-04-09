using Mirror;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(NetworkIdentity))]
public class TriggeredActions : MonoBehaviour //NetworkBehaviour
{
    public UnityEvent entryBehaviour, exitBehaviour;

    public void OnTriggerEnter(Collider other)
    {
        entryBehaviour.Invoke();
        //CmdTriggerEnter();
    }
    public void OnTriggerExit(Collider other)
    {
        exitBehaviour.Invoke();
        //CmdTriggerExit();
    }
/*
    [Command]
    public void CmdTriggerEnter()
    {
        RpcTriggerEnter();
    }

    [ClientRpc]
    public void RpcTriggerEnter()
    {
        entryBehaviour.Invoke();
    }

    [Command]
    public void CmdTriggerExit()
    {
        RpcTriggerExit();
    }

    [ClientRpc]
    public void RpcTriggerExit()
    {
        exitBehaviour.Invoke();
    }
    */
}
