using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oog : MonoBehaviour
{
    bool looking;
    Transform target;

//we're gonna be working with a sphere collider
    //trigger enter
    //value becomes true
    //while true
    //look at thing that entered

    //trigger exit
    //value becomes false

    void OnEnable(){
        target = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        //looking = true;
        target = other.transform;
    }

    void Update(){
        if(target){   
            transform.LookAt(target);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.transform == target){
            //looking = false;
        }
    }


}
