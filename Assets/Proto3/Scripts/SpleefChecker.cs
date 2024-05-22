using System;
using UnityEngine;

public class SpleefChecker : MonoBehaviour
{
    public event Action<MiniGamePlayer> OnPlayerFall;
    public bool Grounded { get; private set; }
    public float maxDistance = 3;

    public MiniGamePlayer myMiniGamePlayer;

    void FixedUpdate()
    {
        if (myMiniGamePlayer == null)
        {
            return;
        }
        transform.position = myMiniGamePlayer.transform.position;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, maxDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            Grounded = true;
            if(hit.transform.GetComponent<Animator>() != null) hit.transform.GetComponent<Animator>().SetTrigger("Fall");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white);
            OnPlayerFall?.Invoke(myMiniGamePlayer);
            Grounded = false;
        }
    }
}
