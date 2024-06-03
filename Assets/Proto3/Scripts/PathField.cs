using System;
using UnityEngine;

public class PathField : MonoBehaviour // Server sided class, disable on clients.
{
    public Action<PlayerDot> OnPlayerFinishedPath;
    public Action<PlayerDot> OnPlayerCollidedWithPath;

    [SerializeField] private Transform startPoint;
    [SerializeField] private PathFinish pathFinish;
    [SerializeField] private PathCollider pathCollider;
    [SerializeField] private Transform playerDotParent;

    public Transform PlayerDotParent => playerDotParent;

    public Transform StartPoint => startPoint;

    private void OnEnable()
    {
        pathFinish.OnPlayerFinishedPath += OnPlayerFinishedPath;
        pathCollider.OnPlayerCollidedWithPath += OnPlayerCollidedWithPath;
    }

    private void OnDisable()
    {
        pathFinish.OnPlayerFinishedPath -= OnPlayerFinishedPath;
        pathCollider.OnPlayerCollidedWithPath -= OnPlayerCollidedWithPath;
    }
}
