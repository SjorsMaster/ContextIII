using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Action<MiniGamePlayer> OnEnemyPlayerHit;

    [SerializeField] private Transform objectThatShouldFollowSword;

    public MiniGamePlayer MyPlayer;

    private void Update()
    {
        if (MyPlayer == null)
        {
            return;
        }

        objectThatShouldFollowSword.SetPositionAndRotation(MyPlayer.RightHandTransform.position, MyPlayer.RightHandTransform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        MiniGamePlayer hitplayer = collision.transform.GetComponent<MiniGamePlayer>();
        
        if (hitplayer == null)
        {
            return;
        }

        if (MyPlayer != null && hitplayer == MyPlayer)
        {
            return;
        }

        OnEnemyPlayerHit?.Invoke(hitplayer);
    }
}