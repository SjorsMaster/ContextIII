using UnityEngine;

public enum PlayerComponentType
{
    Head,
    LeftHand,
    RightHand
}

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private PlayerComponentType playerComponentType;

    public PlayerComponentType PlayerComponentType => playerComponentType;
}