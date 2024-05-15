using SharedSpaces;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    private Animator anim;
    private Transform target;

    private readonly List<TrackedAnchorObject> playersInRange = new();

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        TrackedAnchorObject player = other.GetComponent<TrackedAnchorObject>();
        if (player == null)
        {
            return;
        }

        if (player.name != "Head")
        {
            return;
        }

        playersInRange.Add(player);
        SearchClosestPlayer();

        anim.SetBool("Show", true);
    }

    public void OnTriggerExit(Collider other)
    {
        TrackedAnchorObject player = other.GetComponent<TrackedAnchorObject>();
        if (player == null)
        {
            return;
        }

        if (player.name != "Head")
        {
            return;
        }

        playersInRange.Remove(player);
        if (playersInRange.Count > 0)
        {
            SearchClosestPlayer();
        }
        else
        {
            target = null;
        }

        anim.SetBool("Show", false);
    }

    private void SearchClosestPlayer()
    {
        Transform closest = null;
        foreach (TrackedAnchorObject p in playersInRange)
        {
            if (closest == null)
            {
                closest = p.transform;
            }
            else if (Vector3.Distance(transform.position, p.transform.position) < Vector3.Distance(transform.position, closest.position))
            {
                closest = p.transform;
            }
        }

        target = closest;
    }

}
