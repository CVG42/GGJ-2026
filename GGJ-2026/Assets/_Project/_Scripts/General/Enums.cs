using System;
using UnityEngine;

namespace GGJ
{
    public enum PlatformPowerState
    {
        Power1,
        Power2
    }

    public enum GameState
    {
        OnPlay,
        OnPause,
        OnGameOver
    }

    public enum GameVisualState
    {
        Day,
        Night
    }

    public static class VisualStateEvents
    {
        public static Action<GameVisualState> OnVisualStateChanged;
    }
}
