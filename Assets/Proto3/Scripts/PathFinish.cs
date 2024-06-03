using System;
using UnityEngine;

public class PathFinish : MonoBehaviour // Server sided class, disable on clients.
{
    public Action<PlayerDot> OnPlayerFinishedPath;

    private void OnTriggerEnter(Collider other)
    {
        PlayerDotRef playerDotRef = other.GetComponent<PlayerDotRef>();

        if (playerDotRef != null)
        {
            OnPlayerFinishedPath?.Invoke(playerDotRef.PlayerDot);
        }
    }
}
