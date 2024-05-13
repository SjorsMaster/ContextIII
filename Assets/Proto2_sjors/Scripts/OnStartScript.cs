using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartScript : MonoBehaviour
{
    [SerializeField] UnityEvent unityEvent;
    // Start is called before the first frame update
    void Start()
    {
        unityEvent.Invoke();
    }
}
