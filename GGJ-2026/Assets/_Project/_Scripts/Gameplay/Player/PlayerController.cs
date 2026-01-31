using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace GGJ
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody _rigidbody;
        private float _moveInput;
        private bool _jumpPressed;
        private bool _jumpHeld;

        private float _coyoteCounter;
        private float _jumpBufferCounter;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        private void Start()
        {
            InputManager.Source.OnMove += HandleMove;
            InputManager.Source.OnJumpPressed += HandleJumpPressed;
            InputManager.Source.OnJumpReleased += HandleJumpReleased;
        }

        private void OnDestroy()
        {
            InputManager.Source.OnMove -= HandleMove;
            InputManager.Source.OnJumpPressed -= HandleJumpPressed;
            InputManager.Source.OnJumpReleased -= HandleJumpReleased;
        }

        private void Update()
        {
            GroundCheck();

            if (_isGrounded)
            {
                _coyoteCounter = _stats.CoyoteTime;
            }
            else
            {
                _coyoteCounter -= Time.deltaTime;
            }

            if (_jumpPressed)
            {
                _jumpBufferCounter = _stats.JumpBufferTime;
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            TryJump();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            ApplyGravity();
        }

        private void HandleMove(float input) => _moveInput = input;

        private void HandleJumpPressed()
        {
            _jumpPressed = true;
            _jumpHeld = true;
        }

        private void HandleJumpReleased()
        {
            _jumpHeld = false;
            _jumpPressed = false;
        }

        private void MovePlayer()
        {
            float targetSpeed = _moveInput * _stats.MoveSpeed;
            float speedDifference = targetSpeed - _rigidbody.velocity.x;
            float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? _stats.Acceleration : _stats.Deceleration;
            float movement = speedDifference * accelRate;

            _rigidbody.AddForce(Vector3.right *  movement);
        }

        private void TryJump()
        {
            if (_jumpBufferCounter > 0 && _coyoteCounter > 0)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, 0f);
                _rigidbody.AddForce(Vector3.up * _stats.JumpForce, ForceMode.Impulse);

                _jumpBufferCounter = 0;
                _coyoteCounter = 0;
            }
        }

        private void ApplyGravity()
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity += Vector3.up * Physics.gravity.y * (_stats.FallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (_rigidbody.velocity.y > 0 && !_jumpHeld)
            {
                _rigidbody.velocity += Vector3.up * Physics.gravity.y * (_stats.LowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        private void GroundCheck()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
#endif
    }
}
