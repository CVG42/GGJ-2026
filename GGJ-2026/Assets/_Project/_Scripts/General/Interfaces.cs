using System;

namespace GGJ
{
    public interface IInputSource
    {
        event Action<float> OnMove;
        event Action OnJumpPressed;
        event Action OnJumpReleased;

        event Action OnPlatformPower1;
        event Action OnPlatformPower2;
    }

    public interface IPlatformSource
    {
        bool Power1Unlocked { get; }
        bool Power2Unlocked { get; }

        event Action OnPower1Used;
        event Action OnPower2Used;

        void UnlockPower2();
    }
}
