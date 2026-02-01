using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class PauseUIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _root;
        [SerializeField] private Image _background;
        [SerializeField] private Image _panel;
        [SerializeField] private Image _button1;
        [SerializeField] private Image _button2;

        [Header("Horizontal")]
        [SerializeField] private Color _horizontalBgColor;
        [SerializeField] private Sprite _horizontalPanel;
        [SerializeField] private Sprite _horizontalContinueButton;
        [SerializeField] private Sprite _horizontalExitButton;

        [Header("Vertical")]
        [SerializeField] private Color _verticalBgColor;
        [SerializeField] private Sprite _verticalPanel;
        [SerializeField] private Sprite _verticalContinueButton;
        [SerializeField] private Sprite _verticalExitButton;

        private GameVisualState _currentVisual = GameVisualState.Day;

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += OnHorizontalPower;
            InputManager.Source.OnPlatformPower2 += OnVerticalPower;
            GameManager.Source.OnGameStateChanged += HandleGameState;
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= OnHorizontalPower;
            InputManager.Source.OnPlatformPower2 -= OnVerticalPower;
            GameManager.Source.OnGameStateChanged -= HandleGameState;
        }

        private void OnHorizontalPower()
        {
            SetVisual(GameVisualState.Day);
        }

        private void OnVerticalPower()
        {
            SetVisual(GameVisualState.Night);
        }

        private void HandleGameState(GameState state)
        {
            _root.SetActive(state == GameState.OnPause);

            if (state == GameState.OnPause)
            {
                ApplyVisuals();
            }
        }

        private void SetVisual(GameVisualState visual)
        {
            if (_currentVisual == visual) return;

            _currentVisual = visual;

            if (GameManager.Source.CurrentState == GameState.OnPause)
            {
                ApplyVisuals();
            }
        }

        private void ApplyVisuals()
        {
            switch (_currentVisual)
            {
                case GameVisualState.Day:
                    ApplyDayVisuals();
                    break;

                case GameVisualState.Night:
                    ApplyNightVisuals();
                    break;
            }
        }

        private void ApplyNightVisuals()
        {
            _background.color = _verticalBgColor;
            _panel.sprite = _verticalPanel;
            _button1.sprite = _verticalContinueButton;
            _button2.sprite = _verticalExitButton;
        }

        private void ApplyDayVisuals()
        {
            _background.color = _horizontalBgColor;
            _panel.sprite = _horizontalPanel;
            _button1.sprite = _horizontalContinueButton;
            _button2.sprite = _horizontalExitButton;
        }
    }
}
