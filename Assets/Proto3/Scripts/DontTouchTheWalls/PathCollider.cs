using System;
using UnityEngine;

public class PathCollider : MonoBehaviour // Server sided class, disable on clients.
{
    public Action<PlayerDot> OnPlayerCollidedWithPath;

    private void OnTriggerEnter(Collider other)
    {
        PlayerDotRef playerDotRef = other.GetComponent<PlayerDotRef>();

        if (playerDotRef != null)
        {
            OnPlayerCollidedWithPath?.Invoke(playerDotRef.PlayerDot);
        }
    }
}
