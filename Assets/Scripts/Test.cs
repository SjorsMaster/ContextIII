using UnityEngine;

namespace ContextIII
{
    public class Test : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RelativeSource.Instance.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }
    }
}
