using System;
using DG.Tweening;
using UnityEngine;

namespace GGJ
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private PlatformLimits _limits;

        private int _horizontalDirection = 1;
        private int _verticalDirection = 1;

        private bool _playerOnPlatform;
        private bool _hasMovedHorizontal = false;
        private bool _hasMovedVertical = false;

        private enum ActiveAxis { None, Horizontal, Vertical }
        private ActiveAxis _activeAxis = ActiveAxis.None;

        private Tween _moveTween;

        private void Start()
        {
            GameManager.Source.OnGameStateChanged += HandleGameState;

            PlatformsManager.Source.OnPower1Used += ToggleHorizontal;
            PlatformsManager.Source.OnPower2Used += ToggleVertical;

            InitializeDirections();
        }

        private void OnDestroy()
        {
            PlatformsManager.Source.OnPower1Used -= ToggleHorizontal;
            PlatformsManager.Source.OnPower2Used -= ToggleVertical;

            GameManager.Source.OnGameStateChanged -= HandleGameState;

            _moveTween?.Kill();
        }

        private void HandleGameState(GameState state)
        {
            if (_moveTween == null) return;

            if (state == GameState.OnPause)
            {
                _moveTween.Pause();
            }
            else if (state == GameState.OnPlay)
            {
                _moveTween.Play();
            }
        }

        private void InitializeDirections()
        {
            float x = transform.position.x;
            float y = transform.position.y;

            if (Mathf.Approximately(x, _limits.minX))
            {
                _horizontalDirection = 1;
            }
            else if (Mathf.Approximately(x, _limits.maxX))
            {
                _horizontalDirection = -1;
            }

            if (Mathf.Approximately(y, _limits.minY))
            {
                _verticalDirection = 1;
            }
            else if (Mathf.Approximately(y, _limits.maxY))
            {
                _verticalDirection = -1;
            }
        }

        private void ToggleHorizontal()
        {
            if (!_playerOnPlatform) return;

            if (_activeAxis == ActiveAxis.Horizontal)
            {
                _horizontalDirection *= -1;
                PlatformControllerEvents.OnHorizontalDirectionChanged?.Invoke(_horizontalDirection);
            }
            else if (!_hasMovedHorizontal)
            {
                _hasMovedHorizontal = true;
            }
            else
            {
                _horizontalDirection *= -1;
                PlatformControllerEvents.OnHorizontalDirectionChanged?.Invoke(_horizontalDirection);
            }

            StartMoveHorizontal();
        }

        private void ToggleVertical()
        {
            if (!_playerOnPlatform) return;

            if (_activeAxis == ActiveAxis.Vertical)
            {
                _verticalDirection *= -1;
                PlatformControllerEvents.OnVerticalDirectionChanged?.Invoke(_verticalDirection);
            }
            else if (!_hasMovedVertical)
            {
                _hasMovedVertical = true;
            }
            else
            {
                _verticalDirection *= -1;
                PlatformControllerEvents.OnVerticalDirectionChanged?.Invoke(_verticalDirection);
            }

            StartMoveVertical();
        }

        private void StartMoveHorizontal()
        {
            _activeAxis = ActiveAxis.Horizontal;
            PlatformControllerEvents.OnActivePowerChanged?.Invoke(PlatformPowerState.Power1);
            _moveTween?.Kill();

            float targetX = (_horizontalDirection > 0) ? _limits.maxX : _limits.minX;
            float distance = Mathf.Abs(transform.position.x - targetX);
            float duration = distance / _limits.horizontalSpeed;

            _moveTween = transform.DOMoveX(targetX, duration)
                .SetEase(Ease.InOutSine)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() => _activeAxis = ActiveAxis.None);
        }

        private void StartMoveVertical()
        {
            _activeAxis = ActiveAxis.Vertical;
            PlatformControllerEvents.OnActivePowerChanged?.Invoke(PlatformPowerState.Power2);
            _moveTween?.Kill();

            float targetY = (_verticalDirection > 0) ? _limits.maxY : _limits.minY;
            float distance = Mathf.Abs(transform.position.y - targetY);
            float duration = distance / _limits.verticalSpeed;

            _moveTween = transform.DOMoveY(targetY, duration)
                .SetEase(Ease.InOutSine)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() => _activeAxis = ActiveAxis.None);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerOnPlatform = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerOnPlatform = false;
            }
        }
    }

    public static class PlatformControllerEvents
    {
        public static Action<int> OnHorizontalDirectionChanged;
        public static Action<int> OnVerticalDirectionChanged;

        public static Action<PlatformPowerState> OnActivePowerChanged;
    }
}
