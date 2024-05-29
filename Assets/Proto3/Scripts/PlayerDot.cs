using Mirror;
using Oculus.Interaction;
using UnityEngine;

public class PlayerDot : NetworkBehaviour
{
    [SerializeField] private Grabbable grabbable;

    private int ownerID = 0; // Server only.

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
        ownerID = playerID;
    }

    [Command(requiresAuthority = false)]
    private void CmdRespawn()
    {

    }

    [Server]
    private void Respawn()
    {

    }
}
