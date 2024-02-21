using Meta.WitAi;
using PortalsVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Dissolver : MonoBehaviour
{
    public bool destroyOnTooHeavy = true;
    public bool destroyOnTooSmall = true;

    private Collider col;
	private Rigidbody rb;
	private bool dissolving = false;

    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
		col = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
	}

    public void Dissolve()
    {
        if ( !dissolving )
            StartCoroutine(DoDissolve());
    }

    private IEnumerator DoDissolve( bool destroyWhenDone = false )
    {
        if ( audio ) audio.Play();
		dissolving = true;

		if ( col ) col.enabled = false;
        if (rb)
        {
            Vector3 dir = Vector3.zero;
            dir.x = Random.Range(-1f, 1f);
			dir.y = Random.Range(-1f, 1f);
			dir.z = Random.Range(-1f, 1f);

			rb.velocity = Vector3.zero;
            rb.AddForce(dir);
            rb.useGravity = false;
        }

		Material m = GetComponent<Renderer>().material;
        float t = 1f;

        while ( t > 0 )
        {
			m.SetFloat("_Offset", t);
			t -= Time.deltaTime * .5f;
            yield return null;
        }

		m.SetFloat("_Offset", 0f);

        // Sometimes this is done by another class
        if (destroyWhenDone)
        {
            World w = GetComponentInParent<World>();
            w.Remove(gameObject);
            gameObject.DestroySafely();
        }
	}

    private void Update()
    {
        if ( !dissolving && Time.frameCount % 60 == 0 )
        {
            if (destroyOnTooHeavy && rb.mass > 5f ) 
                StartCoroutine(DoDissolve(true));
			else if (destroyOnTooSmall && transform.localScale.magnitude < 0.1f) 
                StartCoroutine(DoDissolve(true));
		}
    }
}
