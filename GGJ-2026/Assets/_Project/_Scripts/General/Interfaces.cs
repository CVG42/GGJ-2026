using System;

namespace GGJ
{
    public interface IInputSource
    {
        event Action<float> OnMove;
        event Action OnJumpPressed;
        event Action OnJumpReleased;
    }
}
