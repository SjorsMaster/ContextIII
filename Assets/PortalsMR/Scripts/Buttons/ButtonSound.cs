using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : ButtonAction
{
    private AudioSource audioSource;

    private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
    }

    public override void Do()
    {
		audioSource.Play();
		base.Do();
    }
}
