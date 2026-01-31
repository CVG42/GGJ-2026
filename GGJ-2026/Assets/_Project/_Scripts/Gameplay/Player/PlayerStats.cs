using UnityEngine;

namespace GGJ
{
    [CreateAssetMenu(menuName = "GGJ/Player/Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _acceleration = 60f;
        [SerializeField] private float _deceleration = 80f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 14f;
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private float _jumpBufferTime = 0.15f;

        [Header("Gravity")]
        [SerializeField] private float _fallMultiplier = 2.5f;
        [SerializeField] private float _lowJumpMultiplier = 2f;

        public float MoveSpeed => _moveSpeed;
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float JumpForce => _jumpForce;
        public float CoyoteTime => _coyoteTime;
        public float JumpBufferTime => _jumpBufferTime;
        public float FallMultiplier => _fallMultiplier;
        public float LowJumpMultiplier => _lowJumpMultiplier;
    }
}
