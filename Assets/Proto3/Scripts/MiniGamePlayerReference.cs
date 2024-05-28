using UnityEngine;

public class MiniGamePlayerReference : MonoBehaviour
{
    [SerializeField] private MiniGamePlayer player;

    public MiniGamePlayer Player => player;
}
