using System;
using UnityEngine;

public class PathCollider : MonoBehaviour // Server sided class, disable on clients.
{
    public Action<PlayerDot> OnPlayerCollidedWithPath;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerDot playerDot = collision.gameObject.GetComponent<PlayerDot>();

        if (playerDot != null)
        {
            Destroy(playerDot.gameObject);
        }
    }
}
