using UnityEngine;

public class PlayerDotRef : MonoBehaviour
{
    [SerializeField] private PlayerDot playerDot;

    public PlayerDot PlayerDot => playerDot;
}
