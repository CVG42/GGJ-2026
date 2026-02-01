using UnityEngine;

namespace GGJ
{
    public class PlayerSpriteFlip : MonoBehaviour
    {
        private const float FLIP_THRESHOLD = 0.1f;

        [SerializeField] private PlayerController _controller;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _facingRight = true;

        private void Update()
        {
            FlipHandler();
        }

        private void FlipHandler()
        {
            float input = _controller.MoveInput;

            if (Mathf.Abs(input) < FLIP_THRESHOLD) return;

            bool shouldFaceRight = input > 0;

            if (shouldFaceRight != _facingRight)
            {
                Flip(shouldFaceRight);
            }
        }

        private void Flip(bool faceRight)
        {
            _facingRight = faceRight;
            _spriteRenderer.flipX = !faceRight;
        }
    }
}
