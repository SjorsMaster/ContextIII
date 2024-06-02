using UnityEngine;

public class PathField : MonoBehaviour // Server sided class, disable on clients.
{
    [SerializeField] private Transform startPoint;

    public Transform StartPoint => startPoint;

    private void OnTriggerEnter(Collider other)
    {
        PlayerDot playerDot = other.GetComponent<PlayerDot>();

        if (playerDot != null)
        {
            //playerDot.transform.position = startPoint.position;
            Destroy(playerDot.gameObject);
        }
    }
}
