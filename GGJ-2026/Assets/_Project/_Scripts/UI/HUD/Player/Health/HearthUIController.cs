using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class HearthUIController : MonoBehaviour
    {
        private const float DAMAGE_PER_HEART_UNIT = 0.5f;

        [Header("Hearts")]
        [SerializeField] private Image[] _hearts;

        [Header("Platform Power Sprites")]
        [SerializeField] private Sprite _power1Sprite;
        [SerializeField] private Sprite _power2Sprite;

        private int _currentHeartIndex;
        private float _currentHeartFill = 1f;

        private PlatformPowerState _currentPowerState = PlatformPowerState.Power1;

        private void Awake()
        {
            _currentHeartIndex = _hearts.Length - 1;
            _currentHeartFill = _hearts[_currentHeartIndex].fillAmount;
        }

        private void Start()
        {
            PlayerHealthEvents.OnDamageTaken += OnDamageTaken;

            InputManager.Source.OnPlatformPower1 += OnPlatformPower1;
            InputManager.Source.OnPlatformPower2 += OnPlatformPower2;
        }

        private void OnDestroy()
        {
            PlayerHealthEvents.OnDamageTaken -= OnDamageTaken;

            InputManager.Source.OnPlatformPower1 -= OnPlatformPower1;
            InputManager.Source.OnPlatformPower2 -= OnPlatformPower2;
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
                _hearts[_currentHeartIndex].fillAmount = _currentHeartFill;

                damage -= amountToRemove;

                if (_currentHeartFill <= 0f)
                {
                    _currentHeartIndex--;
                    if (_currentHeartIndex >= 0)
                    {
                        _currentHeartFill = _hearts[_currentHeartIndex].fillAmount;
                    }
                }
            }
        }

        private void OnPlatformPower1()
        {
            if (_currentPowerState == PlatformPowerState.Power1) return;

            _currentPowerState = PlatformPowerState.Power1;
            UpdateHeartSprites();
        }

        private void OnPlatformPower2()
        {
            if (_currentPowerState == PlatformPowerState.Power2) return;

            _currentPowerState = PlatformPowerState.Power2;
            UpdateHeartSprites();
        }

        private void UpdateHeartSprites()
        {
            Sprite spriteToUse = _currentPowerState == PlatformPowerState.Power1 ? _power1Sprite : _power2Sprite;

            foreach (var heart in _hearts)
            {
                heart.sprite = spriteToUse;
            }
        }

        public bool IsDead()
        {
            return _currentHeartIndex < 0;
        }
    }
}
