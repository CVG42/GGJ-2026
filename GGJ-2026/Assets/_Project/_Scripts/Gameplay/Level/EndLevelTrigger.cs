using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ
{
    public class EndLevelTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _endGameCanvas;
        [SerializeField] private float _delayBeforeMenu = 2f;

        private bool _triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered) return;

            if (other.CompareTag("Player"))
            {
                _triggered = true;
                HandleEndLevel().Forget();
            }
        }

        private async UniTaskVoid HandleEndLevel()
        {
            _endGameCanvas.SetActive(true);

            GameManager.Source.GameOver();

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeMenu));

            GameManager.Source.ResumeGame();

            SceneManager.LoadScene("MainMenu");
        }
    }
}
