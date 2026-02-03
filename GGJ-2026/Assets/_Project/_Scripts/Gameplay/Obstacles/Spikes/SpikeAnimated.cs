using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;

namespace GGJ
{
    public class SpikeAnimated : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator _animator;

        [Header("Movement")]
        [SerializeField] private float _extendOffsetY = 0.5f;
        [SerializeField] private float _extendOffsetX = 0f;
        [SerializeField] private float _moveDuration = 0.15f;

        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _damageCooldown = 0.5f;

        [Header("Cycle")]
        [SerializeField] private float _timeExtended = 1f;
        [SerializeField] private float _timeRetracted = 1.5f;

        private float _retractedY;
        private float _retractedX;
        private Tween _moveTween;
        private bool _canDamage = true;

        private void Awake()
        {
            _retractedY = transform.position.y;
            _retractedX = transform.position.x;
        }

        private void Start()
        {
            RunCycle(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid RunCycle(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_timeRetracted), cancellationToken: token);

                    if (this == null) return;

                    MoveUp();
                    MoveRight();
                    _animator.SetTrigger("Extend");

                    await UniTask.Delay(TimeSpan.FromSeconds(_timeExtended), cancellationToken: token);

                    if (this == null) return;

                    _animator.SetTrigger("Retract");
                    MoveDown();
                    MoveLeft();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void MoveRight()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveX(_retractedX + _extendOffsetX, _moveDuration);
        }

        private void MoveLeft()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveX(_retractedX, _moveDuration);
        }

        private void MoveUp()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(_retractedY + _extendOffsetY, _moveDuration);
        }

        private void MoveDown()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(_retractedY, _moveDuration);
        }

        private async void DealDamage(PlayerHealth health)
        {
            if (!_canDamage) return;

            _canDamage = false;
            health.TakeDamage(_damage);
            await UniTask.Delay(TimeSpan.FromSeconds(_damageCooldown));
            _canDamage = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    DealDamage(health);
                }
            }
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
