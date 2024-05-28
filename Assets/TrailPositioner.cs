using UnityEngine;

public class TrailPositioner : MonoBehaviour
{
    [SerializeField] private Transform Head;

    private void Update()
    {
        transform.position = Head.position;
    }
}
