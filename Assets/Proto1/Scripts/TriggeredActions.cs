using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggeredActions : MonoBehaviour
{
    
    public UnityEvent triggering;
    public void OnTriggerEnter(Collider other)
    {
            triggering.Invoke();
    }
}
