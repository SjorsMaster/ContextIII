using Mirror;
using UnityEngine;

public class AttachedHandPortal : NetworkBehaviour
{
    private enum Hand
    {
        Left,
        Right,
    }

    [SerializeField] private Hand target;

    private void Update()
    {
        switch (target)
        {
            case Hand.Left:
                LeftHandAttached();
                break;

            case Hand.Right:
                RightHandAttached();
                break;
        }
    }

    private void LeftHandAttached()
    {
        Transform leftHandMirror = TrackedPortalsManager.Instance.LeftHandPortal;
        leftHandMirror.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void RightHandAttached()
    {
        Transform leftHandMirror = TrackedPortalsManager.Instance.RightHandPortal;
        leftHandMirror.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
