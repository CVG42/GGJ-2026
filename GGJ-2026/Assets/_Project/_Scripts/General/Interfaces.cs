using System;
using UnityEngine;

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

    public interface IGameSource
    {
        event Action<GameState> OnGameStateChanged;
        GameState CurrentState { get; }

        void PauseGame();
        void ResumeGame();
        void GameOver();
    }

    public interface IVisualsSource
    {
        bool IsDay {  get; }
        bool IsTransitioning { get; }

        void RegisterSpriteRenderer(SpriteRenderer renderer, Sprite daySprite, Sprite nightSprite);
        void UnregisterSpriteRenderer(SpriteRenderer renderer);
        void PauseCycle();
        void ResumeCycle();
    }
}
