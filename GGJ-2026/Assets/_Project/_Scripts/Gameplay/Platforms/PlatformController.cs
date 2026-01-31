using UnityEngine;

namespace GGJ
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private PlatformLimits _limits;

        private int _horizontalDirection = 1;
        private int _verticalDirection = 1;

        private bool _playerOnPlatform;
        private bool _movingHorizontal;
        private bool _movingVertical;

        private void Start()
        {
            PlatformsManager.Source.OnPower1Used += ToggleHorizontal;
            PlatformsManager.Source.OnPower2Used += ToggleVertical;
        }

        private void OnDestroy()
        {
            PlatformsManager.Source.OnPower1Used -= ToggleHorizontal;
            PlatformsManager.Source.OnPower2Used -= ToggleVertical;
        }

        private void Update()
        {
            if (!_playerOnPlatform) return;

            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void ToggleHorizontal()
        {
            if (!_playerOnPlatform) return;

            _horizontalDirection *= -1;
            _movingHorizontal = true;
        }

        private void ToggleVertical()
        {
            if (!_playerOnPlatform) return;

            _verticalDirection *= -1;
            _movingVertical = true;
        }

        private void HandleHorizontalMovement()
        {
            if (!_movingHorizontal) return;

            float nextX = transform.position.x + _horizontalDirection * _limits.horizontalSpeed * Time.deltaTime;

            if (nextX >= _limits.maxX)
            {
                nextX = _limits.maxX;
                _movingHorizontal = false;
            }
            else if (nextX <= _limits.minX)
            {
                nextX = _limits.minX;
                _movingHorizontal = false;
            }

            transform.position = new Vector3(nextX, transform.position.y, transform.position.z);
        }

        private void HandleVerticalMovement()
        {
            if (!_movingVertical) return;

            float nextY = transform.position.y + _verticalDirection * _limits.verticalSpeed * Time.deltaTime;

            if (nextY >= _limits.maxY)
            {
                nextY = _limits.maxY;
                _movingVertical = false;
            }
            else if (nextY <= _limits.minY)
            {
                nextY = _limits.minY;
                _movingVertical = false;
            }

            transform.position = new Vector3(transform.position.x, nextY, transform.position.z);
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
}
