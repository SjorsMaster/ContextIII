using UnityEngine;

namespace ContextIII
{
    public class TrackedDevice : MonoBehaviour
    {
        [field: SerializeField] public Transform TrackedHeadTransform { get; private set; }
        [field: SerializeField] public Transform TrackedLeftHandTransform { get; private set; }
        [field: SerializeField] public Transform TrackedRightHandTransform { get; private set; }
    }
}
