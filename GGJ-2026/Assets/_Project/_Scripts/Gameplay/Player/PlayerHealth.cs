using System;
using UnityEngine;

namespace GGJ
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 5;

        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            Debug.Log($"Player HP: {_currentHealth}");

            PlayerHealthEvents.OnDamageTaken?.Invoke(amount);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void RestoreFullHealth()
        {
            _currentHealth = _maxHealth;
        }

        private void Die()
        {
            PlayerHealthEvents.OnPlayerDied?.Invoke();
        }
    }

    public static class PlayerHealthEvents
    {
        public static Action<int> OnDamageTaken;
        public static Action OnPlayerDied;
    }
}
