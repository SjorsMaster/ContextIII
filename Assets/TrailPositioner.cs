using UnityEngine;

public class TrailPositioner : MonoBehaviour
{
    [SerializeField] private Transform Head;

    private void Update()
    {
        transform.SetPositionAndRotation(Head.position, Quaternion.identity);
    }
}
