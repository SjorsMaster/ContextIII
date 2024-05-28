using SharedSpaces;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Transform target;

    private readonly List<AnchoredObject> playersInRange = new();

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
        AnchoredObject player = other.GetComponent<AnchoredObject>();
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
        AnchoredObject player = other.GetComponent<AnchoredObject>();
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

        for (int i = 0; i < playersInRange.Count; i++)
        {
            AnchoredObject p = playersInRange[i];
            if (p == null)
            {
                playersInRange.RemoveAt(i);
                i--;
                continue;
            }
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
