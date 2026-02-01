using UnityEngine;

namespace GGJ
{
    public class PlayerFallController : MonoBehaviour
    {
        [SerializeField] private Transform _checkpoint;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Reset"))
                return;

            Respawn();
        }

        private void Respawn()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            transform.position = _checkpoint.position;
        }

        public void SetCheckpoint(Transform newCheckpoint)
        {
            _checkpoint = newCheckpoint;
        }
    }
}
