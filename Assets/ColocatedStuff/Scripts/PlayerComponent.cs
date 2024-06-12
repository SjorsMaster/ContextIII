using Mirror;
using SharedSpaces;
using SharedSpaces.Managers;
using UnityEngine;

public enum PlayerComponentType
{
    Head,
    LeftHand,
    RightHand
}

public class PlayerComponent : NetworkBehaviour
{
    [SerializeField] private PlayerComponentType playerComponentType;

    public PlayerComponentType PlayerComponentType => playerComponentType;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isOwned)
        {
            return;
        }

        AnchoredObject obj = GetComponent<AnchoredObject>();

        switch (playerComponentType)
        {
            case PlayerComponentType.Head:
                LocalPlayerManager.SetHead(obj);
                break;
            case PlayerComponentType.LeftHand:
                LocalPlayerManager.SetLeftHand(obj);
                break;
            case PlayerComponentType.RightHand:
                LocalPlayerManager.SetRightHand(obj);
                break;
        }

    }
}