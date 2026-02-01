using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ
{
    public class SpikesStatic : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _damageCooldownMs = 2000;

        private bool _canDamage = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                DealDamage(health);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                DealDamage(health);
            }
        }

        private async void DealDamage(PlayerHealth health)
        {
            if (!_canDamage) return;

            _canDamage = false;
            health.TakeDamage(_damage);

            await UniTask.Delay(_damageCooldownMs);
            _canDamage = true;
        }
    }
}
