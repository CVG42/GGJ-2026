using UnityEngine;

namespace GGJ
{
    public class TurretTrigger : MonoBehaviour
    {
        [SerializeField] private TurretShooter _turret;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _turret.StartShooting();
            }
        }
    }
}
