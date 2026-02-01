using UnityEngine;

namespace GGJ
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerController _controller;

        private GameVisualState _currentVisualState = GameVisualState.Day;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int VisualStateHash = Animator.StringToHash("VisualState");
        private static readonly int ToDayHash = Animator.StringToHash("ToDay");
        private static readonly int ToNightHash = Animator.StringToHash("ToNight");

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += SwitchToDay;
            InputManager.Source.OnPlatformPower2 += SwitchToNight;

            _animator.SetInteger(VisualStateHash, 0);
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= SwitchToDay;
            InputManager.Source.OnPlatformPower2 -= SwitchToNight;
        }

        private void Update()
        {
            UpdateLocomotionParams();
        }

        private void UpdateLocomotionParams()
        {
            float speed = Mathf.Abs(_controller.CurrentHorizontalSpeed);
            _animator.SetFloat(SpeedHash, speed);
        }

        private void SwitchToDay()
        {
            if (_currentVisualState == GameVisualState.Day) return;

            _currentVisualState = GameVisualState.Day;
            _animator.SetInteger(VisualStateHash, 0);
            _animator.SetTrigger(ToDayHash);
        }

        private void SwitchToNight()
        {
            if (_currentVisualState == GameVisualState.Night) return;

            _currentVisualState = GameVisualState.Night;
            _animator.SetInteger(VisualStateHash, 1);
            _animator.SetTrigger(ToNightHash);
        }
    }
}
