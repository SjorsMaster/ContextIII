using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class MassByScale : MonoBehaviour
{
    const float TOO_HEAVY_TO_HANDLE = 5f;

    private Vector3 oldScale = Vector3.zero;
    private Rigidbody rb;
    private float baseMass = 1;

    private XRGrabInteractable grabInteract;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        baseMass = rb.mass;

        grabInteract = GetComponent<XRGrabInteractable>();
	}

    // Update is called once per frame
    void Update()
    {
        if ( transform.localScale.x != oldScale.x )
        {
			oldScale = transform.localScale;
			rb.mass = baseMass * oldScale.magnitude;

            if (grabInteract)
                grabInteract.enabled = ( rb.mass < TOO_HEAVY_TO_HANDLE );
		}
    }
}
