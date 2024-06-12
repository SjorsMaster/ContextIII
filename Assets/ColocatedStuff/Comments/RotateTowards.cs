using SharedSpaces;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private Transform target;

    private readonly List<PlayerComponent> playersInRange = new();

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

        SearchClosestPlayer();
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerComponent player = other.GetComponent<PlayerComponent>();
        if (player == null)
        {
            return;
        }

        if (player.PlayerComponentType != PlayerComponentType.Head)
        {
            return;
        }

        playersInRange.Add(player);

        anim.SetBool("Show", true);
    }

    public void OnTriggerExit(Collider other)
    {
        PlayerComponent player = other.GetComponent<PlayerComponent>();
        if (player == null)
        {
            return;
        }

        if (player.PlayerComponentType != PlayerComponentType.Head)
        {
            return;
        }

        playersInRange.Remove(player);

        anim.SetBool("Show", false);
    }

    private void SearchClosestPlayer()
    {
        Transform closest = null;

        for (int i = 0; i < playersInRange.Count; i++)
        {
            PlayerComponent p = playersInRange[i];
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
