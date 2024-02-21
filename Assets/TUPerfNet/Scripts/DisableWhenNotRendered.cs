using UnityEngine;

namespace TU.PerfNet
{
    public class DisableWhenNotRendered : MonoBehaviour
    {
        public ParticleSystem handVFX;

        private Vector3 m_LastLocation;
        private bool shouldPlay;

        private void Awake()
        {
            if (!handVFX)
            {
                handVFX = GetComponent<ParticleSystem>();
            }
        }

        private void LateUpdate()
        {
            if (transform.position != m_LastLocation || transform.position != Vector3.zero)
            {
                if (shouldPlay)
                {
                    handVFX.Play();
                    shouldPlay = false;
                }
            }
            else
            {
                handVFX.Stop();
                shouldPlay = true;
            }
            m_LastLocation = transform.position;
        }

    }
}