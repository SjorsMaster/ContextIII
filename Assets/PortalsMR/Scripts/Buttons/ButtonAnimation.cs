using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ButtonAnimation : ButtonAction
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Do()
    {
        animator.SetTrigger("press");
		base.Do();
    }

    public override void Undo()
    {
		animator.SetTrigger("release");
		base.Undo();
    }
}
