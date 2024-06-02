using Mirror;
using Oculus.Interaction;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDot : NetworkBehaviour
{
    [SerializeField] private NetworkTransformBase networkTransformBase;
    [SerializeField] private Grabbable grabbable;

    private Vector3 respawnPoint;

    public int OwnerID { get; private set;  } = -1; // Server only.

    #region Event Handlers
    private void Grabbable_WhenPointerEventRaised(PointerEvent pointerEvent)
    {
        if (pointerEvent.Type == PointerEventType.Select)
        {
            CmdSetMiniGamePlayerID(MiniGamePlayer.LocalPlayerID);
        }
        else if (pointerEvent.Type == PointerEventType.Unselect)
        {
            CmdRespawn();
        }
    }
    #endregion

    [ClientCallback]
    private void OnEnable()
    {
        grabbable.WhenPointerEventRaised += Grabbable_WhenPointerEventRaised;
    }

    [ClientCallback]
    private void OnDisable()
    {
        grabbable.WhenPointerEventRaised -= Grabbable_WhenPointerEventRaised;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetMiniGamePlayerID(int playerID)
    {
        OwnerID = playerID;
    }

    [Command(requiresAuthority = false)]
    private void CmdRespawn()
    {
        Respawn();
    }

    [Server]
    private async void Respawn()
    {
        // Another class handles changing the Syncdirection to ServerToClient on unselecting the object, here we wait for it to be done.
        while (networkTransformBase.syncDirection != SyncDirection.ServerToClient)
        {
            await Task.Yield();
        }

        transform.position = respawnPoint;
        OwnerID = -1;
    }

    [Server]
    public void SetRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }
}
