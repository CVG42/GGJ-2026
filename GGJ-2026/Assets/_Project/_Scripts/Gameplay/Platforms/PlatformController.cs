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

        private enum ActiveAxis { None, Horizontal, Vertical };
        private ActiveAxis _activeAxis = ActiveAxis.None;

        private void Start()
        {
            PlatformsManager.Source.OnPower1Used += ToggleHorizontal;
            PlatformsManager.Source.OnPower2Used += ToggleVertical;

            InitializeDirections();
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

        private void InitializeDirections()
        {
            float x = transform.position.x;
            float y = transform.position.y;

            if (Mathf.Approximately(x, _limits.minX))
            {
                _horizontalDirection = -1;
            }
            else if (Mathf.Approximately(x, _limits.maxX))
            {
                _horizontalDirection = 1;
            }

            if (Mathf.Approximately(y, _limits.minY))
            {
                _verticalDirection = -1;
            }
            else if (Mathf.Approximately(y, _limits.maxY))
            {
                _verticalDirection = 1;
            }
        }

        private void ToggleHorizontal()
        {
            if (!_playerOnPlatform) return;

            _horizontalDirection *= -1;
            _activeAxis = ActiveAxis.Horizontal;
            _movingHorizontal = true;
            _movingVertical = false;
        }

        private void ToggleVertical()
        {
            if (!_playerOnPlatform) return;

            _verticalDirection *= -1;
            _activeAxis = ActiveAxis.Vertical;
            _movingVertical = true;
            _movingHorizontal = false;
        }

        private void HandleHorizontalMovement()
        {
            if (!_movingHorizontal || _activeAxis != ActiveAxis.Horizontal) return;

            float nextX = transform.position.x + _horizontalDirection * _limits.horizontalSpeed * Time.deltaTime;

            if (nextX >= _limits.maxX)
            {
                nextX = _limits.maxX;
                _movingHorizontal = false;
                _activeAxis = ActiveAxis.None;
            }
            else if (nextX <= _limits.minX)
            {
                nextX = _limits.minX;
                _movingHorizontal = false;
                _activeAxis = ActiveAxis.None;
            }

            transform.position = new Vector3(nextX, transform.position.y, transform.position.z);
        }

        private void HandleVerticalMovement()
        {
            if (!_movingVertical || _activeAxis != ActiveAxis.Vertical) return;

            float nextY = transform.position.y + _verticalDirection * _limits.verticalSpeed * Time.deltaTime;

            if (nextY >= _limits.maxY)
            {
                nextY = _limits.maxY;
                _movingVertical = false;
                _activeAxis = ActiveAxis.None;
            }
            else if (nextY <= _limits.minY)
            {
                nextY = _limits.minY;
                _movingVertical = false;
                _activeAxis = ActiveAxis.None;
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
