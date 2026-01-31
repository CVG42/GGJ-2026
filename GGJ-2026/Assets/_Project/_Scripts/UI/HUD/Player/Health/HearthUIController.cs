using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class HearthUIController : MonoBehaviour
    {
        private const float DAMAGE_PER_HEART_UNIT = 0.5f;
        
        [SerializeField] private Image[] hearts;

        private int _currentHeartIndex;
        private float _currentHeartFill = 1f;

        private void Awake()
        {
            _currentHeartIndex = hearts.Length - 1;
            _currentHeartFill = hearts[_currentHeartIndex].fillAmount;
        }

        private void Start()
        {
            PlayerHealthEvents.OnDamageTaken += OnDamageTaken;
        }

        private void OnDestroy()
        {
            PlayerHealthEvents.OnDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(int damageAmount)
        {
            float visualDamage = damageAmount * DAMAGE_PER_HEART_UNIT;
            ApplyVisualDamage(visualDamage);
        }

        private void ApplyVisualDamage(float damage)
        {
            while (damage > 0 && _currentHeartIndex >= 0)
            {
                float amountToRemove = Mathf.Min(damage, _currentHeartFill);

                _currentHeartFill -= amountToRemove;
                hearts[_currentHeartIndex].fillAmount = _currentHeartFill;

                damage -= amountToRemove;

                if (_currentHeartFill <= 0f)
                {
                    _currentHeartIndex--;
                    if (_currentHeartIndex >= 0)
                    {
                        _currentHeartFill = hearts[_currentHeartIndex].fillAmount;
                    }
                }
            }
        }

        public bool IsDead()
        {
            return _currentHeartIndex < 0;
        }
    }
}
