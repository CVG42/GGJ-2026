using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace GGJ
{
    public class LevelVisualsManager : Singleton<IVisualsSource>, IVisualsSource
    {
        [Header("Lighting")]
        [SerializeField] private Light _directionalLight;
        [SerializeField] private Color _dayLightColor = Color.white;
        [SerializeField] private Color _nightLightColor = new Color(0.4f, 0.5f, 0.8f);
        [SerializeField] private float _dayLightIntensity = 1f;
        [SerializeField] private float _nightLightIntensity = 0.3f;

        [Header("Day/Night Cycle Settings")]
        [SerializeField] private float _dayDuration = 30f;
        [SerializeField] private float _nightDuration = 30f;
        [SerializeField] private float _transitionDuration = 2f;
        [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Dictionary<SpriteRenderer, (Sprite day, Sprite night)> _registeredRenderers = new();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isDay = true;
        private bool _isTransitioning = false;

        public bool IsDay => _isDay;
        public bool IsTransitioning => _isTransitioning;

        private void Start()
        {
            ApplyVisualsImmediate(_isDay);
            StartDayNightCycle().Forget();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTaskVoid StartDayNightCycle()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay((int)((_dayDuration - _transitionDuration) * 1000), cancellationToken: token);
                    await TransitionToNight(token);

                    await UniTask.Delay((int)((_nightDuration - _transitionDuration) * 1000), cancellationToken: token);
                    await TransitionToDay(token);
                }
                catch (System.OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async UniTask TransitionToNight(CancellationToken token)
        {
            if (_isTransitioning) return;

            _isTransitioning = true;
            await TransitionVisuals(false, token);
            _isTransitioning = false;
        }

        private async UniTask TransitionToDay(CancellationToken token)
        {
            if (_isTransitioning) return;

            _isTransitioning = true;
            await TransitionVisuals(true, token);
            _isTransitioning = false;
        }

        private async UniTask TransitionVisuals(bool toDay, CancellationToken token)
        {
            if (_transitionDuration <= 0)
            {
                _isDay = toDay;
                ApplyVisualsImmediate(toDay);
                return;
            }

            Color startColor = _directionalLight.color;
            float startIntensity = _directionalLight.intensity;
            Color targetColor = toDay ? _dayLightColor : _nightLightColor;
            float targetIntensity = toDay ? _dayLightIntensity : _nightLightIntensity;

            var spriteTasks = new List<UniTask>();

            foreach (var kvp in _registeredRenderers)
            {
                if (kvp.Key != null)
                {
                    var sprites = kvp.Value;
                    Sprite targetSprite = toDay ? sprites.day : sprites.night;

                    spriteTasks.Add(TransitionSprite(kvp.Key, targetSprite, _transitionDuration, token));
                }
            }

            float elapsedTime = 0f;
            while (elapsedTime < _transitionDuration && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                float t = _transitionCurve.Evaluate(elapsedTime / _transitionDuration);

                if (_directionalLight != null)
                {
                    _directionalLight.color = Color.Lerp(startColor, targetColor, t);
                    _directionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            await UniTask.WhenAll(spriteTasks);

            if (!token.IsCancellationRequested)
            {
                _isDay = toDay;
                ApplyVisualsImmediate(toDay);
            }
        }

        private async UniTask TransitionSprite(SpriteRenderer renderer, Sprite targetSprite, float duration, CancellationToken token)
        {
            if (renderer == null || duration <= 0 || token.IsCancellationRequested) return;

            Sprite originalSprite = renderer.sprite;
            Material originalMaterial = renderer.material;

            Material fadeMaterial = new Material(Shader.Find("Sprites/Default"));
            fadeMaterial.CopyPropertiesFromMaterial(originalMaterial);
            renderer.material = fadeMaterial;

            float elapsedTime = 0f;
            while (elapsedTime < duration && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                float t = _transitionCurve.Evaluate(elapsedTime / duration);

                if (elapsedTime < duration / 2f)
                {
                    Color color = renderer.color;
                    color.a = 1f - (t * 2f);
                    renderer.color = color;
                }
                else
                {
                    if (renderer.sprite != targetSprite)
                    {
                        renderer.sprite = targetSprite;
                        renderer.color = new Color(1, 1, 1, 0);
                    }

                    Color color = renderer.color;
                    color.a = (t - 0.5f) * 2f;
                    renderer.color = color;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (!token.IsCancellationRequested && renderer != null)
            {
                renderer.color = Color.white;
                renderer.material = originalMaterial;
                renderer.sprite = targetSprite;
            }
        }

        private void ApplyVisualsImmediate(bool isDay)
        {
            if (_directionalLight != null)
            {
                _directionalLight.color = isDay ? _dayLightColor : _nightLightColor;
                _directionalLight.intensity = isDay ? _dayLightIntensity : _nightLightIntensity;
            }

            foreach (var kvp in _registeredRenderers)
            {
                var renderer = kvp.Key;
                var sprites = kvp.Value;

                if (renderer != null)
                {
                    renderer.sprite = isDay ? sprites.day : sprites.night;
                    renderer.color = Color.white;
                }
            }
        }

        public void RegisterSpriteRenderer(SpriteRenderer renderer, Sprite daySprite, Sprite nightSprite)
        {
            if (renderer == null) return;

            _registeredRenderers[renderer] = (daySprite, nightSprite);

            renderer.sprite = _isDay ? daySprite : nightSprite;
            renderer.color = Color.white;
        }

        public void UnregisterSpriteRenderer(SpriteRenderer renderer)
        {
            if (renderer != null)
            {
                _registeredRenderers.Remove(renderer);
            }
        }

        public void PauseCycle()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void ResumeCycle()
        {
            if (_cancellationTokenSource?.IsCancellationRequested ?? true)
            {
                StartDayNightCycle().Forget();
            }
        }
    }
}
