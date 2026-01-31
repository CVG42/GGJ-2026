using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GGJ
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        [SerializeField] private Vector3 offset = new Vector3(0, 2f, -10f);

        [SerializeField] private float smoothTimeX = 0.1f;
        [SerializeField] private float smoothTimeY = 0.15f;

        private float velocityX;
        private float velocityY;

        private void LateUpdate()
        {
            if (_player == null) return;

            Vector3 desiredPos = _player.position + offset;

            float smoothX = Mathf.SmoothDamp(
                transform.position.x,
                desiredPos.x,
                ref velocityX,
                smoothTimeX);

            float smoothY = Mathf.SmoothDamp(
                transform.position.y,
                desiredPos.y,
                ref velocityY,
                smoothTimeY);

            transform.position = new Vector3(smoothX, smoothY, offset.z);
        }
    }
}
