using System.Collections.Generic;
using UnityEngine;

namespace bullet
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private int cacheSize = 200;

        [SerializeField]
        private Bullet bulletPrefab;

        private List<Bullet> bulletCache;
        private int currentIndex;

        private void Start()
        {
            CreateBulletCache();
        }

        private void CreateBulletCache()
        {
            bulletCache = new List<Bullet>(cacheSize);
            var spawnPosition = new Vector2(-1000, -1000);

            for (var i = 0; i < cacheSize; i++)
            {
                var bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity, transform);
                bullet.gameObject.SetActive(false);
                bulletCache.Add(bullet);
            }
        }

        public void FireBullet(GameObject ignore, Vector2 startPosition, Vector2 direction)
        {
            var bullet = bulletCache[currentIndex];
            currentIndex = (currentIndex + 1) % cacheSize;

            bullet.PrimeBullet(ignore, startPosition, direction);
            bullet.gameObject.SetActive(true);
            bullet.Fire();
        }
    }
}