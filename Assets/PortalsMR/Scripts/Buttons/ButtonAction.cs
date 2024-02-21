using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonAction : MonoBehaviour
{
	public bool Performed { get; private set; }

	public virtual void Do()
	{
		Performed = true;
	}

	public virtual void Undo()
	{
		Performed = false;
	}
}
