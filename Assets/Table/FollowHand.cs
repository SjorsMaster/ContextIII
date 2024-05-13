using Mirror;
using UnityEngine;

public class FollowHand : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdatePaddle))] private Vector3 paddlePos;

    [SerializeField] private Rigidbody _paddleRb;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _boundsA, _boundsB;

    /// <summary>
    /// Lets you set the hand object to be tracked.
    /// </summary>
    /// <param name="newHand">The hand or whatever needs to be tracked</param>
    public void SetHand(Transform newHand) => _hand = newHand;

    // Update is called once per frame
    void Update()
    {
        if (!_hand) 
            return;

        Vector3 handsToLocal = transform.InverseTransformPoint(_hand.position);
        Vector3 targetPos = ClampToBounds(handsToLocal);
        _paddleRb.MovePosition(transform.TransformPoint(targetPos));
    }

    Vector3 ClampToBounds(Vector3 input)
    {
        // Check if the input is within the bounds of their local positions
        input.x = Mathf.Clamp(input.x, _boundsA.localPosition.x, _boundsB.localPosition.x);
        input.z = Mathf.Clamp(input.z, _boundsA.localPosition.z, _boundsB.localPosition.z);
        return input;
    }

    private void UpdatePaddle(Vector3 oldPos, Vector3 newPos)
    {
        _paddleRb.MovePosition(newPos);
    }

    [Command]
    private void Cmd_UpdatePaddle(Vector3 newPos)
    {

    }
}
