using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _initialSize = 20;

        private Queue<GameObject> _pool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                CreateBullet();
            }
        }

        private GameObject CreateBullet()
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
            return bullet;
        }

        public GameObject GetBullet()
        {
            if (_pool.Count == 0)
                CreateBullet();

            GameObject bullet = _pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }
}
