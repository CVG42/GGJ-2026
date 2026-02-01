using Cinemachine;
using UnityEngine;

namespace GGJ
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource _impulseSource;

        private void Start()
        {
            PlayerHealthEvents.OnDamageTaken += OnDamageTaken;
        }

        private void OnDestroy()
        {
            PlayerHealthEvents.OnDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(int damage)
        {
            _impulseSource.GenerateImpulse();
        }
    }
}
