using System;
using UnityEngine;

namespace GGJ
{
    public class InputManager : Singleton<IInputSource>, IInputSource
    {
        public event Action<float> OnMove;
        public event Action OnJumpPressed;
        public event Action OnJumpReleased;

        private void Update()
        {
            HandleMovement();
            HandleJump();
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
    }
}
