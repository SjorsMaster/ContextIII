using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOnTouch : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        this.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    public void OnTriggerExit(Collider other)
    {
        this.GetComponent<Renderer>().material.color = Color.white;
    }
}
