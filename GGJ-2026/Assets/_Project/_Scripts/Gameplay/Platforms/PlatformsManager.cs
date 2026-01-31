using System;
using UnityEngine;

namespace GGJ
{
    public class PlatformsManager : Singleton<IPlatformSource>, IPlatformSource
    {
        public bool Power1Unlocked { get; private set; } = true;
        public bool Power2Unlocked { get; private set; } = false;

        public event Action OnPower1Used;
        public event Action OnPower2Used;

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += TryUsePower1;
            InputManager.Source.OnPlatformPower2 += TryUsePower2;
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= TryUsePower1;
            InputManager.Source.OnPlatformPower2 -= TryUsePower2;
        }

        private void TryUsePower1()
        {
            if (Power1Unlocked) 
            {
                OnPower1Used?.Invoke();
            }
        }

        private void TryUsePower2()
        {
            if (Power2Unlocked)
            {
                OnPower2Used?.Invoke();
            }
        }

        public void UnlockPower2() => Power2Unlocked = true;
    }
}
