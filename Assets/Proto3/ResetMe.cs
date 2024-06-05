using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMe : MonoBehaviour
{
    Vector3 startpos;
    // Start is called before the first frame update
    void Awake()
    {
        startpos = transform.position;  

    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        transform.position = startpos;
    }
}
