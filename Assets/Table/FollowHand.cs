using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    [SerializeField] GameObject _hand, _paddle, _boundsA, _boundsB;
    [SerializeField] float _speed;

    Rigidbody _rigidbody;

    void Start(){
        _rigidbody = _paddle.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.MovePosition(CheckBounds(_hand.transform.position));
    }

    Vector3 CheckBounds(Vector3 input){

        float tmp1 = input.x > _boundsA.transform.position.x ? (input.x < _boundsB.transform.position.x ? input.x : _boundsB.transform.position.x) : _boundsA.transform.position.x;
        float tmp2 = input.z > _boundsA.transform.position.z ? (input.z < _boundsB.transform.position.z ? input.z : _boundsB.transform.position.z) : _boundsA.transform.position.z;

        print($"{tmp1},{input.y},{tmp2}");
        return new Vector3(tmp1, input.y, tmp2);
    }
}
