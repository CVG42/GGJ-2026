using UnityEngine;

namespace GGJ
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            PlayerFallController fallController = other.GetComponent<PlayerFallController>();

            if (fallController == null) return;

            Transform point = _respawnPoint != null ? _respawnPoint : transform;
            fallController.SetCheckpoint(point);
        }
    }
}
