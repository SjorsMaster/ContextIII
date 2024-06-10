using UnityEngine;

public class AirhockeyGoal : MonoBehaviour
{
    public event System.Action OnGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puck"))
        {
            OnGoal?.Invoke();
        }
    }
}
