using System;
using UnityEngine;

namespace GGJ
{
    public class GameManager : Singleton<IGameSource>, IGameSource
    {
        public event Action<GameState> OnGameStateChanged;

        public GameState CurrentState { get; private set; } = GameState.OnPlay;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentState == GameState.OnPlay)
                {
                    PauseGame();
                }
                else if (CurrentState == GameState.OnPause)
                {
                    ResumeGame();
                }
            }
        }

        public void PauseGame()
        {
            SetState(GameState.OnPause);
        }

        public void ResumeGame()
        {
            SetState(GameState.OnPlay);
        }

        public void GameOver()
        {
            SetState(GameState.OnGameOver);
        }

        private void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }
    }
}
