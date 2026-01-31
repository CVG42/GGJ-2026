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
        private bool isActive;

        private void Start()
        {
            if (startDelay > 0)
            {
                StartShootingWithDelay().Forget();
            }
        }

        private async UniTaskVoid StartShootingWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken: this.GetCancellationTokenOnDestroy());

            StartShooting();
        }

        public void StartShooting()
        {
            if (isActive) return;

            isActive = true;
            shootingCTS = new CancellationTokenSource();
            ShootLoopAsync(shootingCTS.Token).Forget();
        }

        public void StopShooting()
        {
            if (!isActive) return;

            isActive = false;
            shootingCTS.Cancel();
            shootingCTS.Dispose();
            shootingCTS = null;
        }

        private async UniTaskVoid ShootLoopAsync(CancellationToken token)
        {
            try
            {
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
            shootingCTS?.Cancel();
            shootingCTS?.Dispose();
        }
    }
}
