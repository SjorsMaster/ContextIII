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

    [SerializeField] private UnityEvent<PlayerComponent> onPlayerCollisionEnter;
    [SerializeField] private UnityEvent<PlayerComponent> onPlayerCollisionExit;

    public event Action<PlayerComponent> OnPlayerCollisionEnter;
    public event Action<PlayerComponent> OnPlayerCollisionExit;

    #region OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (syncDirection == SyncDirection.ServerToClient)
        {
            ServerOnTriggerEnter(other);
        }
        else
        {
            ClientOnTriggerEnter(other);
        }
    }

    [ServerCallback]
    private void ServerOnTriggerEnter(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            onPlayerTriggerEnter?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerTriggerEnter(netID);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerTriggerEnter(uint netID)
    {
        if (authority)
        {
            return;
        }

        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            onPlayerTriggerEnter?.Invoke(playerComponent);
        }
    }

    [ClientCallback]
    private void ClientOnTriggerEnter(Collider other)
    {
        if (!authority)
        {
            return;
        }

        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            onPlayerTriggerEnter?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            CmdOnTriggerEnter(netID);
        }
    }

    [Command]
    private void CmdOnTriggerEnter(uint netID, NetworkConnectionToClient sender = null)
    {
        if (NetworkServer.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerEnter?.Invoke(playerComponent);
            onPlayerTriggerEnter?.Invoke(playerComponent);

            RpcOnPlayerTriggerEnter(netID);
        }
    }
    #endregion

    #region OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        if (syncDirection == SyncDirection.ServerToClient)
        {
            ServerOnTriggerExit(other);
        }
        else
        {
            ClientOnTriggerExit(other);
        }
    }

    [ServerCallback]
    private void ServerOnTriggerExit(Collider other)
    {
        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerExit?.Invoke(playerComponent);
            onPlayerTriggerExit?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerTriggerExit(netID);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerTriggerExit(uint netID)
    {
        if (authority)
        {
            return;
        }

        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerExit?.Invoke(playerComponent);
            onPlayerTriggerExit?.Invoke(playerComponent);
        }
    }

    [ClientCallback]
    private void ClientOnTriggerExit(Collider other)
    {
        if (!authority)
        {
            return;
        }

        PlayerComponent playerComponent = other.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerTriggerExit?.Invoke(playerComponent);
            OnPlayerTriggerExit?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            CmdOnTriggerExit(netID);
        }
    }

    [Command]
    private void CmdOnTriggerExit(uint netID, NetworkConnectionToClient sender = null)
    {
        if (NetworkServer.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerTriggerExit?.Invoke(playerComponent);
            onPlayerTriggerExit?.Invoke(playerComponent);

            RpcOnPlayerTriggerExit(netID);
        }
    }
    #endregion

    #region OnCollisionEnter
    private void OnCollisionEnter(Collision collision)
    {
        if (syncDirection == SyncDirection.ServerToClient)
        {
            ServerOnCollisionEnter(collision);
        }
        else
        {
            ClientOnCollisionEnter(collision);
        }
    }

    [ServerCallback]
    private void ServerOnCollisionEnter(Collision collision)
    {
        PlayerComponent playerComponent = collision.collider.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerCollisionEnter?.Invoke(playerComponent);
            onPlayerCollisionEnter?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerCollisionEnter(netID);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerCollisionEnter(uint netID)
    {
        if (authority)
        {
            return;
        }

        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerCollisionEnter?.Invoke(playerComponent);
            onPlayerCollisionEnter?.Invoke(playerComponent);
        }
    }

    [ClientCallback]
    private void ClientOnCollisionEnter(Collision collision)
    {
        if (!authority)
        {
            return;
        }

        PlayerComponent playerComponent = collision.collider.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerCollisionEnter?.Invoke(playerComponent);
            onPlayerCollisionEnter?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            CmdOnCollisionEnter(netID);
        }
    }

    [Command]
    private void CmdOnCollisionEnter(uint netID, NetworkConnectionToClient sender = null)
    {
        if (NetworkServer.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerCollisionEnter?.Invoke(playerComponent);
            onPlayerCollisionEnter?.Invoke(playerComponent);

            RpcOnPlayerCollisionEnter(netID);
        }
    }
    #endregion

    #region OnCollisionExit
    private void OnCollisionExit(Collision collision)
    {
        if (syncDirection == SyncDirection.ServerToClient)
        {
            ServerOnCollisionExit(collision);
        }
        else
        {
            ClientOnCollisionExit(collision);
        }
    }

    [ServerCallback]
    private void ServerOnCollisionExit(Collision collision)
    {
        PlayerComponent playerComponent = collision.collider.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerCollisionExit?.Invoke(playerComponent);
            onPlayerCollisionExit?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            RpcOnPlayerCollisionExit(netID);
        }
    }

    [ClientRpc]
    public void RpcOnPlayerCollisionExit(uint netID)
    {
        if (authority)
        {
            return;
        }

        if (NetworkClient.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerCollisionExit?.Invoke(playerComponent);
            onPlayerCollisionExit?.Invoke(playerComponent);
        }
    }

    [ClientCallback]
    private void ClientOnCollisionExit(Collision collision)
    {
        if (!authority)
        {
            return;
        }

        PlayerComponent playerComponent = collision.collider.GetComponent<PlayerComponent>();
        if (playerComponent != null)
        {
            OnPlayerCollisionExit?.Invoke(playerComponent);
            onPlayerCollisionExit?.Invoke(playerComponent);

            uint netID = playerComponent.GetComponent<NetworkIdentity>().netId;
            CmdOnCollisionExit(netID);
        }
    }

    [ClientCallback]
    private void CmdOnCollisionExit(uint netID, NetworkConnectionToClient sender = null)
    {
        if (NetworkServer.spawned.TryGetValue(netID, out NetworkIdentity identity))
        {
            PlayerComponent playerComponent = identity.GetComponent<PlayerComponent>();
            OnPlayerCollisionExit?.Invoke(playerComponent);
            onPlayerCollisionExit?.Invoke(playerComponent);

            RpcOnPlayerCollisionExit(netID);
        }
    }
    #endregion
}
