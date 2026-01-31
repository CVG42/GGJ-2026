using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class HearthSpriteController : MonoBehaviour
    {
        [Header("Hearts")]
        [SerializeField] private Image[] _hearts;

        [Header("Platform Power Sprites")]
        [SerializeField] private Sprite _power1Sprite;
        [SerializeField] private Sprite _power2Sprite;

        private PlatformPowerState _currentPowerState = PlatformPowerState.Power1;

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += OnPlatformPower1;
            InputManager.Source.OnPlatformPower2 += OnPlatformPower2;
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= OnPlatformPower1;
            InputManager.Source.OnPlatformPower2 -= OnPlatformPower2;
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
    }
}
