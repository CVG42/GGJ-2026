using UnityEngine;

namespace GGJ
{
    public class TurretStopTrigger : MonoBehaviour
    {
        [SerializeField] private TurretShooter turret;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                turret.DisableTurret();
            }
        }
    }
}
