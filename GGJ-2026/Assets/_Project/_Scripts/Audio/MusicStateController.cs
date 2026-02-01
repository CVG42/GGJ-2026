using UnityEngine;

namespace GGJ
{
    public class MusicStateController : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _daySource;
        [SerializeField] private AudioSource _nightSource;

        [Header("Tracks")]
        [SerializeField] private AudioClip _dayMusic;
        [SerializeField] private AudioClip _nightMusic;

        [Header("Settings")]
        [SerializeField] private float _fadeSpeed = 2f;
        [SerializeField] private float _maxVolume = 0.5f;

        private GameVisualState _currentState;

        private void Start()
        {
            SetupSources();
            SetState(GameVisualState.Day);

            VisualStateEvents.OnVisualStateChanged += SetState;
        }

        private void OnDestroy()
        {
            VisualStateEvents.OnVisualStateChanged -= SetState;
        }

        private void SetupSources()
        {
            _daySource.clip = _dayMusic;
            _nightSource.clip = _nightMusic;

            _daySource.loop = true;
            _nightSource.loop = true;

            _daySource.volume = 0;
            _nightSource.volume = 0;

            _daySource.Play();
            _nightSource.Play();
        }

        private void Update()
        {
            UpdateVolumes();
        }

        public void SetState(GameVisualState state)
        {
            _currentState = state;
        }

        private void UpdateVolumes()
        {
            float targetDay = _currentState == GameVisualState.Day ? _maxVolume : 0;
            float targetNight = _currentState == GameVisualState.Night ? _maxVolume : 0;

            _daySource.volume = Mathf.MoveTowards(_daySource.volume, targetDay, _fadeSpeed * Time.deltaTime);
            _nightSource.volume = Mathf.MoveTowards(_nightSource.volume, targetNight, _fadeSpeed * Time.deltaTime);
        }
    }
}
