using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookOrderer : MonoBehaviour
{

    public UnityEvent achieved;

    [SerializeField]
    int index = 0, goal = 4;
    
    public void CheckList(int identifier){
        if(index == identifier)
            index++;
        else
            index = 0;

        if(index == goal){
            achieved.Invoke();
            Destroy(this);
        }

    }

    
}
