using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace GGJ
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _zOffset;
        [SerializeField] private float _smoothTime = 0.3f;

        private Cinemachine3rdPersonFollow _thirdPersonFollow;
        private CancellationTokenSource _cts;

        private void Start()
        {
            if (_camera != null)
            {
                _thirdPersonFollow = _camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _thirdPersonFollow != null)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                SmoothZoomAsync(_zOffset, _cts.Token).Forget();
            }
        }

        private async UniTask SmoothZoomAsync(float targetZOffset, CancellationToken token)
        {
            try
            {
                float elapsedTime = 0f;
                Vector3 currentOffset = _thirdPersonFollow.ShoulderOffset;
                float startZ = currentOffset.z;

                while (elapsedTime < _smoothTime)
                {
                    token.ThrowIfCancellationRequested();

                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / _smoothTime);
                    float easedT = EaseInOutCubic(t);

                    float newZ = Mathf.Lerp(startZ, targetZOffset, easedT);

                    Vector3 offset = _thirdPersonFollow.ShoulderOffset;
                    offset.z = newZ;
                    _thirdPersonFollow.ShoulderOffset = offset;

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                Vector3 finalOffset = _thirdPersonFollow.ShoulderOffset;
                finalOffset.z = targetZOffset;
                _thirdPersonFollow.ShoulderOffset = finalOffset;
            }
            catch (System.OperationCanceledException) { }
        }

        private float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
