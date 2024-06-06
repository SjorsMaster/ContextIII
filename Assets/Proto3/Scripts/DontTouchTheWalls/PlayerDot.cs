using Mirror;
using Oculus.Interaction;
using System;
using UnityEngine;

public class PlayerDot : NetworkBehaviour
{
    public Action<PlayerDot> OnGrabRelease;

    [SerializeField] private NetworkTransformBase networkTransformBase;
    [SerializeField] private RayInteractable rayInteractable;
    [SerializeField] private LayerMask hitMask;

    public Transform SpawnTransform;

    public int OwnerID { get; private set; } = -1; // Server only.

    private RayInteractor interactor = null;

    #region Event Handlers
    private void Grabbable_WhenPointerEventRaised(PointerEvent pointerEvent)
    {
        switch (pointerEvent.Type)
        {
            case PointerEventType.Select:
                CmdSetMiniGamePlayerID(MiniGamePlayer.LocalPlayer.PlayerID);
                interactor = (RayInteractor)pointerEvent.Data;
                CmdSetSyncDirectionToClientToServer();
                break;

            case PointerEventType.Unselect:
                interactor = null;
                CmdSetSyncDirectionToServerToClient();
                CmdOnGrabRelease();
                break;
        }
    }
    #endregion

    [ClientCallback]
    private void OnEnable()
    {
        rayInteractable.WhenPointerEventRaised += Grabbable_WhenPointerEventRaised;
    }

    [ClientCallback]
    private void OnDisable()
    {
        rayInteractable.WhenPointerEventRaised -= Grabbable_WhenPointerEventRaised;
    }

    [ClientCallback]
    private void Update()
    {
        if (interactor == null)
        {
            return;
        }

        if (Physics.Raycast(interactor.Origin, interactor.Forward, out RaycastHit hit, Mathf.Infinity, hitMask))
        {
            transform.position = hit.point;
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetMiniGamePlayerID(int playerID)
    {
        OwnerID = playerID;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetSyncDirectionToClientToServer(NetworkConnectionToClient sender = null)
    {
        networkTransformBase.netIdentity.AssignClientAuthority(sender);
        networkTransformBase.syncDirection = SyncDirection.ClientToServer;

        RpcSetSyncDirectionToClientToServer();
    }

    [ClientRpc]
    private void RpcSetSyncDirectionToClientToServer()
    {
        networkTransformBase.syncDirection = SyncDirection.ClientToServer;
    }


    [Command(requiresAuthority = false)]
    private void CmdSetSyncDirectionToServerToClient()
    {
        networkTransformBase.netIdentity.RemoveClientAuthority();
        networkTransformBase.syncDirection = SyncDirection.ServerToClient;

        RpcSetSyncDirectionToServerToClient();
    }

    [ClientRpc]
    private void RpcSetSyncDirectionToServerToClient()
    {
        networkTransformBase.syncDirection = SyncDirection.ServerToClient;
    }

    [Command(requiresAuthority = false)]
    private void CmdOnGrabRelease()
    {
        OnGrabRelease?.Invoke(this);
    }
}
