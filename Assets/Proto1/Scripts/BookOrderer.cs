using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookOrderer : MonoBehaviour
{

    public UnityEvent achieved;

    [SerializeField]
    private GameObject[] orderList;
    int orderIndex = 0;
    
    public void CheckList(GameObject passed){
        if(passed == orderList[orderIndex])
            orderIndex++;
        else
            orderIndex = 0;

        if(orderIndex>orderList.Length){
            achieved.Invoke();
            Destroy(this);
        }

    }

    
}
