using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Action<MiniGamePlayer> OnPlayerHit;
    
    public void InvokeOnPlayerHit(MiniGamePlayer miniGamePlayer)
    {
        OnPlayerHit?.Invoke(miniGamePlayer);
    }
}