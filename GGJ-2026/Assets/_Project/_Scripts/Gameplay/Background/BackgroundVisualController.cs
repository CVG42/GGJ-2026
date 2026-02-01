using UnityEngine;

namespace GGJ
{
    public class BackgroundVisualController : MonoBehaviour
    {
        [Header("Background")]
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private Sprite _daySprite;
        [SerializeField] private Sprite _nightSprite;

        [Header("Lighting")]
        [SerializeField] private Light _directionalLight;
        [SerializeField] private Color _dayLightColor = Color.white;
        [SerializeField] private Color _nightLightColor = new Color(0.4f, 0.5f, 0.8f);
        [SerializeField] private float _dayLightIntensity = 1f;
        [SerializeField] private float _nightLightIntensity = 0.3f;

        private GameVisualState _currentState = GameVisualState.Day;

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += SetDay;
            InputManager.Source.OnPlatformPower2 += SetNight;

            ApplyVisuals();
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= SetDay;
            InputManager.Source.OnPlatformPower2 -= SetNight;
        }

        private void SetDay()
        {
            if (_currentState == GameVisualState.Day) return;

            _currentState = GameVisualState.Day;
            ApplyVisuals();
        }

        private void SetNight()
        {
            if (_currentState == GameVisualState.Night) return;

            _currentState = GameVisualState.Night;
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            bool isDay = _currentState == GameVisualState.Day;

            _backgroundRenderer.sprite = _currentState == GameVisualState.Day ? _daySprite : _nightSprite;

            _directionalLight.color = isDay ? _dayLightColor : _nightLightColor;
            _directionalLight.intensity = isDay ? _dayLightIntensity : _nightLightIntensity;
        }
    }
}
