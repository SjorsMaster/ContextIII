using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicFollower : MonoBehaviour
{
    [SerializeField] private bool unparent = true;
    [SerializeField] private Transform target;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        if (unparent)
        {
            transform.SetParent(null);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(target.position);
    }
}
