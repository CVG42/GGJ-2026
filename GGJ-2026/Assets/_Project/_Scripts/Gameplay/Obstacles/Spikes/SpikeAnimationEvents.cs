using UnityEngine;

namespace GGJ
{
    public class SpikeAnimationEvents : MonoBehaviour
    {
        [SerializeField] private Collider _spikeCollider;

        public void EnableCollider()
        {
            _spikeCollider.enabled = true;
        }

        public void DisableCollider()
        {
            _spikeCollider.enabled = false;
        }
    }
}
