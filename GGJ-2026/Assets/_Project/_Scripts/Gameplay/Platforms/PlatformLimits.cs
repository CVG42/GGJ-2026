using UnityEngine;

namespace GGJ
{
    [CreateAssetMenu(menuName = "GGJ/Platforms/Platform Limits")]
    public class PlatformLimits : ScriptableObject
    {
        [Header("Horizontal Limits")]
        public float minX = -5f;
        public float maxX = 5f;

        [Header("Vertical Limits")]
        public float minY = 0f;
        public float maxY = 5f;

        [Header("Speed")]
        public float horizontalSpeed = 3f;
        public float verticalSpeed = 3f;
    }
}
