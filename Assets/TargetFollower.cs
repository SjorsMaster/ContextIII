using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private bool detachOnStart = true;

    [SerializeField] private bool followXPos = true;
    [SerializeField] private bool followYPos = true;
    [SerializeField] private bool followZPos = true;

    [SerializeField] private bool followXRot = true;
    [SerializeField] private bool followYRot = true;
    [SerializeField] private bool followZRot = true;

    private void Start()
    {
        if (detachOnStart)
        {
            transform.SetParent(null);
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 newPos = transform.position;
        if (followXPos)
        {
            newPos.x = target.position.x;
        }
        if (followYPos)
        {
            newPos.y = target.position.y;
        }
        if (followZPos)
        {
            newPos.z = target.position.z;
        }
        transform.position = newPos;

        Vector3 newRot = transform.eulerAngles;
        if (followXRot)
        {
            newRot.x = target.eulerAngles.x;
        }
        if (followYRot)
        {
            newRot.y = target.eulerAngles.y;
        }
        if (followZRot)
        {
            newRot.z = target.eulerAngles.z;
        }
        transform.eulerAngles = newRot;
    }
}
