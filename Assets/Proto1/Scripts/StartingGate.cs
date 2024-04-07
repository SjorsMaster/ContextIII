using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartingGate : MonoBehaviour
{
    int total = 0;
    public UnityEvent reachedval;
    public void SetValue(int value)
    {
        total+= value;
            if(total == 2)
            reachedval.Invoke();

    }
}
