using System;
using UnityEngine;

public class PathFinish : MonoBehaviour // Server sided class, disable on clients.
{
    public Action<PlayerDot> OnPlayerFinishedPath;

    private void OnTriggerEnter(Collider other)
    {
        PlayerDot playerDot = other.GetComponent<PlayerDot>();

        if (playerDot != null)
        {
            OnPlayerFinishedPath?.Invoke(playerDot);
        }
    }
}
