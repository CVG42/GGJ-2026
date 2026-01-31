using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace GGJ
{
    public class TurretShooter : MonoBehaviour
    {
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private Transform firePoint;

        [Header("Shooting")]
        [SerializeField] private int bulletsPerBurst = 3;
        [SerializeField] private float timeBetweenShots = 0.2f;
        [SerializeField] private float burstCooldown = 2f;
        [SerializeField] private float startDelay = 0f;

        private CancellationTokenSource shootingCTS;
        private bool isEnabledByGameplay;
        private bool isPaused;

        private void Start()
        {
            GameManager.Source.OnGameStateChanged += HandleGameState;
        }

        public void EnableTurret()
        {
            isEnabledByGameplay = true;
            TryStartShooting();
        }

        public void DisableTurret()
        {
            isEnabledByGameplay = false;
            StopShooting();
        }

        private void HandleGameState(GameState state)
        {
            isPaused = state == GameState.OnPause;

            if (isPaused)
            {
                StopShooting();
            }
            else
            {
                TryStartShooting();
            }
        }

        private void TryStartShooting()
        {
            if (!isEnabledByGameplay) return;
            if (isPaused) return;
            if (shootingCTS != null) return;

            shootingCTS = new CancellationTokenSource();
            ShootLoopAsync(shootingCTS.Token).Forget();
        }

        private void StopShooting()
        {
            if (shootingCTS == null) return;

            shootingCTS.Cancel();
            shootingCTS.Dispose();
            shootingCTS = null;
        }

        private async UniTaskVoid ShootLoopAsync(CancellationToken token)
        {
            try
            {
                if (startDelay > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken: token);
                }

                while (!token.IsCancellationRequested)
                {
                    for (int i = 0; i < bulletsPerBurst; i++)
                    {
                        Shoot();
                        await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenShots), cancellationToken: token);
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(burstCooldown), cancellationToken: token);
                }
            }
            catch (OperationCanceledException) { }
        }

        private void Shoot()
        {
            GameObject bullet = bulletPool.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            bullet.GetComponent<Bullet>().Init(bulletPool);
        }

        private void OnDestroy()
        {
            GameManager.Source.OnGameStateChanged -= HandleGameState;
            StopShooting();
        }
    }
}
