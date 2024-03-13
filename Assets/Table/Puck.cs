using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puck : MonoBehaviour
{
    public UnityEvent HitGoalA, HitGoalB;
    GameObject _lastHit;
    [SerializeField] PointTracker _tracker;
    Rigidbody _rb;

    Vector3 _startpos;

    private void OnCollisionEnter(Collision collision) {
        print($"{collision.gameObject.name} hit!");
        if (collision.gameObject.tag == "GoalA") { _tracker.ChangeScore(0, 1); _rb.isKinematic = true; transform.position = _startpos; _rb.isKinematic = false; }
        if (collision.gameObject.tag == "GoalB") { _tracker.ChangeScore(1, 0); _rb.isKinematic = true; transform.position = _startpos; _rb.isKinematic = false; }
        if (collision.gameObject.tag == "Handle") _lastHit = collision.gameObject;
    }

    IEnumerator ScaleMe(GameObject input, GameObject trigger) {
        trigger.GetComponent<Collider>().enabled = false;
        input.transform.localScale = input.transform.localScale * 2;
        yield return new WaitForSeconds(8);
        trigger.GetComponent<Collider>().enabled = true;
        input.transform.localScale = input.transform.localScale / 2;
    }

    private void OnTriggerEnter(Collider other) {
        print($"{other.gameObject.name} triggered!");
        if(_lastHit)StartCoroutine(ScaleMe(_lastHit, other.gameObject));
    }

    // Start is called before the first frame update
    void Start()
    {
        _startpos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

}
