using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    //temp solution til I see yvar lol
    public BookOrderer handler;

    private void OnTriggerEnter(Collider other)
    {
        handler.CheckList(this.gameObject);
    }

    //make delegate that bookordered listens to
/*    public delegate void OnTouched(); //
    public static event OnTouched onTouched;

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        onTouched?.Invoke();
    }

    //ontrigger enter>> give gameobejct itself
    */

}
