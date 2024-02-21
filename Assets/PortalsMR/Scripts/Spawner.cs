using PortalsVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public float spawnDelay = 10f;
	public GameObject toSpawn;
    public Vector3 spawnOffset = Vector3.zero;
    public int maxSpawn = 1;
    public bool autoRespawn = true;

    private List<GameObject> spawnedObjects;

	private World parentWorld;
    private AudioSource audio;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(spawnDelay);

		parentWorld = GetComponentInParent<World>();
        audio = GetComponent<AudioSource>();
		spawnedObjects = new List<GameObject>(maxSpawn);
        if ( autoRespawn && maxSpawn > 0 ) {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedObjects != null && Time.frameCount % 60 == 0 )
        {
            for (int i = 0; i < spawnedObjects.Count; ++i)
            {
                if (spawnedObjects[i] == null)
                {
                    spawnedObjects.RemoveAt(i--);
                    continue;
                }
            }
            if (spawnedObjects.Count == 0 && autoRespawn) 
                Spawn();
        }
    }

    void Spawn()
    {
        if (!toSpawn) return;

        GameObject o = GameObject.Instantiate(toSpawn, transform.position + spawnOffset, Quaternion.identity);
        parentWorld.Add(o);
		spawnedObjects.Add(o);

		PortalTraveller pt = o.GetComponent<PortalTraveller>();
		if (pt) pt.activeWorld = parentWorld.name;

        if ( audio )
        {
            audio.Play();
		}
    }
}