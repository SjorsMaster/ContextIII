using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DraaiDeurBV : MonoBehaviour
{
    public List<UnityEvent> entryBehaviour, exitBehaviour;
    int tracker=0, cooldownS = 9;
    bool canTrigger = true, canExit = false; //To avoid repeat calling when moving back an forth

    public void OnTriggerEnter(Collider other)
    {
        if(canTrigger == true){
            canTrigger = false;
            canExit = true;
            if(entryBehaviour.Count >= tracker+1)entryBehaviour[tracker].Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(canExit == true){
            canExit = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        if(exitBehaviour.Count >= tracker+1)exitBehaviour[tracker].Invoke();
        yield return new WaitForSeconds(cooldownS);
        canTrigger = true;
        tracker++;
    }
}
