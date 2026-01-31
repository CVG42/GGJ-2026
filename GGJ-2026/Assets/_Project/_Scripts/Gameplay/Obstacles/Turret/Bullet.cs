using UnityEngine;

namespace GGJ
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifeTime = 5f;

        private BulletPool pool;
        private float timer;
        private bool _isPaused;

        public void Init(BulletPool bulletPool)
        {
            pool = bulletPool;
            timer = 0f;
        }

        private void OnEnable()
        {
            GameManager.Source.OnGameStateChanged += HandleGameState;
            timer = 0f;
        }

        private void Update()
        {
            if (_isPaused) return;

            transform.position += transform.right * speed * Time.deltaTime;

            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth player))
            {
                player.TakeDamage(damage);
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            pool.ReturnBullet(gameObject);
        }

        private void HandleGameState(GameState state)
        {
            _isPaused = state == GameState.OnPause;
        }

        private void OnDisable()
        {
            GameManager.Source.OnGameStateChanged -= HandleGameState;
        }
    }
}
