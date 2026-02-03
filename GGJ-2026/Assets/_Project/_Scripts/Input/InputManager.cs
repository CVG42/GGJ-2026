using System;
using UnityEngine;

namespace GGJ
{
    public class InputManager : Singleton<IInputSource>, IInputSource
    {
        public event Action<float> OnMove;
        public event Action OnJumpPressed;
        public event Action OnJumpReleased;

        public event Action OnPlatformPower1;
        public event Action OnPlatformPower2;

        private void Update()
        {
            HandleMovement();
            HandleJump();
            HandlePlatforms();
        }

        private void HandleMovement()
        {
            float move = Input.GetAxisRaw("Horizontal");
            OnMove?.Invoke(move);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                OnJumpPressed?.Invoke();
            }

            if (Input.GetButtonUp("Jump"))
            {
                OnJumpReleased?.Invoke();
            }
        }

        private void HandlePlatforms()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnPlatformPower1?.Invoke();
                VisualStateEvents.OnVisualStateChanged?.Invoke(GameVisualState.Day);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            { 
                OnPlatformPower2?.Invoke();
                VisualStateEvents.OnVisualStateChanged?.Invoke(GameVisualState.Night);
            }
        }
    }
}
