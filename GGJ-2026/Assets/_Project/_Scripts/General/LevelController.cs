using System.Collections;
using UnityEngine;

namespace GGJ
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Transform _levelStartPoint;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private HearthUIController _heartsUI;

        private void Start()
        {
            PlayerHealthEvents.OnPlayerDied += OnPlayerDied;
        }

        private void OnDestroy()
        {
            PlayerHealthEvents.OnPlayerDied -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            StartCoroutine(ResetRun());
        }

        private IEnumerator ResetRun()
        {
            yield return new WaitForSeconds(0.5f);

            ResetPlayer();
            ResetHealth();
        }

        private void ResetPlayer()
        {
            var controller = FindFirstObjectByType<PlayerController>();
            controller.transform.position = _levelStartPoint.position;
        }

        private void ResetHealth()
        {
            _playerHealth.RestoreFullHealth();
            _heartsUI.ResetHeartsVisual();
        }
    }
}
