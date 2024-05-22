using UnityEngine;
using UnityEngine.Events;

public class LocalTrigger : MonoBehaviour
{
    public UnityEvent entryBehaviour, exitBehaviour;

    public void OnTriggerEnter(Collider other)
    {
        entryBehaviour.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        exitBehaviour.Invoke();
    }
}
