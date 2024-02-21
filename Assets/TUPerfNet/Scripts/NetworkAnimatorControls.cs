using UnityEngine;
using Mirror;

namespace TU.PerfNet
{
    public class NetworkAnimatorControls : NetworkBehaviour
    {
        public bool triggerAnimation;
        public NetworkAnimator animator;

        private void Update()
        {
            if (triggerAnimation)
            {
                triggerAnimation = false;
                animator.SetTrigger("Trigger");
            }
        }
    }
}