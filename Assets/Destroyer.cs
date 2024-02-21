using Meta.WitAi;
using PortalsVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Destroyer : MonoBehaviour
{
	public HashSet<GameObject> beingDestroyed = new HashSet<GameObject>();

	private void OnTriggerEnter(Collider other)
	{
		if (beingDestroyed.Contains(other.gameObject)) return;

		PortalTraveller pt = other.GetComponent<PortalTraveller>();
		if ( pt && !pt.isPlayer )
		{			
			other.gameObject.SendMessage("Dissolve");
			beingDestroyed.Add(other.gameObject);
			StartCoroutine(RemoveAfter(other.gameObject, pt, 3f));
		}
	}

	IEnumerator RemoveAfter(GameObject target, PortalTraveller pt, float t)
	{
		
		yield return new WaitForSeconds(t);
		World.worlds[pt.activeWorld].Remove(target);
		beingDestroyed.Remove(target);
		target.DestroySafely();
	}
}
