using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

public class NetworkedPlayerTrigger : NetworkBehaviour
{
    [SerializeField] private UnityEvent<PlayerComponent> onPlayerTriggerEnter;
    [SerializeField] private UnityEvent<PlayerComponent> onPlayerTriggerExit;

    public event Action<PlayerComponent> OnPlayerTriggerEnter;
    public event Action<PlayerComponent> OnPlayerTriggerExit;

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            
            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerTriggerEnter(netID);
        }
    }

    [Server]
    private void OnTriggerExit(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            OnPlayerTriggerEnter?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerTriggerExit(netID);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerTriggerEnter(uint netID)
    {
        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            onPlayerTriggerEnter?.Invoke(playerComponent);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerTriggerExit(uint netID)
    {
        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerExit?.Invoke(playerComponent);
            onPlayerTriggerExit?.Invoke(playerComponent);
        }
    }
}
