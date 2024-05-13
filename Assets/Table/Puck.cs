using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Puck : MonoBehaviour
{
    public UnityEvent HitGoalA, HitGoalB;
    GameObject _lastHit;
    [SerializeField] PointTracker _tracker;
    Rigidbody _rb;

    Vector3 _startpos;

    private void OnCollisionEnter(Collision collision)
    {
        print($"{collision.gameObject.name} hit!");
        if (collision.gameObject.CompareTag("GoalA")) OnGoal(0, 1);
        if (collision.gameObject.CompareTag("GoalB")) OnGoal(1, 0);
        if (collision.gameObject.CompareTag("Handle")) _lastHit = collision.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print($"{other.gameObject.name} triggered!");
        //if (_lastHit) StartCoroutine(ScaleMe(_lastHit, other.gameObject));
    }

    private void OnGoal(int score1, int score2)
    {
        _tracker.ChangeScore(score1, score2);
        transform.position = _startpos;
        _rb.velocity = Vector3.zero;
    }

    IEnumerator ScaleMe(GameObject input, GameObject trigger)
    {
        trigger.GetComponent<Collider>().enabled = false;
        input.transform.localScale = input.transform.localScale * 2;
        yield return new WaitForSeconds(8);
        trigger.GetComponent<Collider>().enabled = true;
        input.transform.localScale = input.transform.localScale / 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startpos = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

}
