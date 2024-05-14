using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    bool looking;
    Transform target;
    [SerializeField] Animator anim;

    void OnEnable(){
        target = null;
        //anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        anim.SetBool("Show",true);
        target = other.transform;
    }

    void Update(){
        if(target){   
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
    }

    public void OnTriggerExit(Collider other)
    {
        anim.SetBool("Show",false);
        //target = null;
    }
}
