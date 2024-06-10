using Mirror;
using Oculus.Interaction;
using UnityEngine;

public class RayPointPositioner : NetworkBehaviour
{
    [SerializeField] private NetworkTransformBase networkTransformBase;
    [SerializeField] private RayInteractable rayInteractable;
    [SerializeField] private LayerMask hitMask;

    [SerializeField] private Vector3 rayDirection;
    [SerializeField] private float rayDistance;

    public int OwnerID { get; private set; } = -1; // Server only.

    private RayInteractor interactor = null;

    #region Event Handlers
    private void Grabbable_WhenPointerEventRaised(PointerEvent pointerEvent)
    {
        switch (pointerEvent.Type)
        {
            case PointerEventType.Select:
                interactor = (RayInteractor)pointerEvent.Data;
                CmdSetSyncDirectionToClientToServer();
                break;

            case PointerEventType.Unselect:
                interactor = null;
                CmdSetSyncDirectionToServerToClient();
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

        if (Physics.Raycast(interactor.Origin, rayDirection, out RaycastHit hit, rayDistance, hitMask))
        {
            transform.position = hit.point;
        }
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
}
