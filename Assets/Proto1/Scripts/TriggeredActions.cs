using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggeredActions : MonoBehaviour
{
    
    public UnityEvent entryBehaviour,exitBehaviour;
    public void OnTriggerEnter(Collider other)
    {
            entryBehaviour.Invoke();
    }
    public void OnTriggerExit(Collider other)
    {
            exitBehaviour.Invoke();
    }
}
