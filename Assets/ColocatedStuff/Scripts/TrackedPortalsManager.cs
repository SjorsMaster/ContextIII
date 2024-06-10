using SharedSpaces.Singletons;
using UnityEngine;

public class TrackedPortalsManager : Singleton<TrackedPortalsManager>
{
    [field: SerializeField] public Transform LeftHandPortal { get; private set; }
    [field: SerializeField] public Transform RightHandPortal { get; private set; }
}