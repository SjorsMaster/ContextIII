using System;
using UnityEngine;
using UnityEngine.Events;

public class MiniGamePlayerTrigger : MonoBehaviour
{
    public UnityEvent<MiniGamePlayer> OnEntryBehaviour;
    public UnityEvent<MiniGamePlayer> OnExitBehaviour;

    public void OnTriggerEnter(Collider other)
    {
        if (OnEntryBehaviour == null)
        {
            return;
        }

        MiniGamePlayerReference reference = other.GetComponent<MiniGamePlayerReference>();
        if (reference == null)
        {
            return;
        }

        MiniGamePlayer player = reference.Player;
        if (player == null)
        {
            return;
        }

        OnEntryBehaviour.Invoke(player);
    }

    public void OnTriggerExit(Collider other)
    {
        if (OnExitBehaviour == null)
        {
            return;
        }

        MiniGamePlayerReference reference = other.GetComponent<MiniGamePlayerReference>();
        if (reference == null)
        {
            return;
        }

        MiniGamePlayer player = reference.Player;
        if (player == null)
        {
            return;
        }

        OnExitBehaviour.Invoke(player);
    }
}
