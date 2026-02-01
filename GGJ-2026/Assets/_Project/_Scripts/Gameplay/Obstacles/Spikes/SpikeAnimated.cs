using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using DG.Tweening;

namespace GGJ
{
    public class SpikeAnimated : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator _animator;

        [Header("Movement")]
        [SerializeField] private float _extendOffsetY = 0.5f;
        [SerializeField] private float _moveDuration = 0.15f;

        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _damageCooldown = 0.5f;

        [Header("Cycle")]
        [SerializeField] private float _timeExtended = 1f;
        [SerializeField] private float _timeRetracted = 1.5f;

        private float _retractedY;
        private Tween _moveTween;
        private bool _canDamage = true;

        private void Awake()
        {
            _retractedY = transform.position.y;
        }

        private void Start()
        {
            RunCycle().Forget();
        }

        private async UniTaskVoid RunCycle()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_timeRetracted));

                MoveUp();
                _animator.SetTrigger("Extend");

                await UniTask.Delay(TimeSpan.FromSeconds(_timeExtended));

                _animator.SetTrigger("Retract");
                MoveDown();
            }
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
