using System.Collections.Generic;
using UnityEngine;

namespace bullet
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private Bullet bulletPrefab;
        
        [SerializeField]
        private int cacheSize = 200;
        
        private List<Bullet> bulletCache;
        private int currentIndex;

        private void Start()
        {
            CreateBulletCache();
        }

        private void CreateBulletCache()
        {
            bulletCache = new List<Bullet>(cacheSize);
            Vector2 spawnPosition = new Vector2(-1000, -1000);

            for (int i = 0; i < cacheSize; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity, transform);
                bullet.gameObject.SetActive(false);
                bulletCache.Add(bullet);
            }
        }

        public void FireBullet(GameObject ignore, Vector2 startPosition, Vector2 direction)
        {
            Bullet bullet = bulletCache[currentIndex];
            currentIndex = (currentIndex + 1) % cacheSize;

            bullet.PrimeBullet(ignore, startPosition, direction);
            bullet.gameObject.SetActive(true);
            bullet.Fire();
        }
    }
}