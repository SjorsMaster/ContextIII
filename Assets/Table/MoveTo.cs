using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField] float _speed =2f;
    // Update is called once per frame
    void Update()
    {
        var step = _speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
    }
}
