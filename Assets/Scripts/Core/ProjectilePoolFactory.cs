using Core.Interfaces;
using Projectiles;
using UnityEngine;

namespace Core
{
    public class ProjectilePoolFactory : MonoBehaviour, IProjectileFactory
    {
        public static ProjectilePoolFactory Instance { get; private set; }

        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private int _initialPoolSize = 20;

        private ObjectPool<Projectile> _pool;
        private Transform _globalContainer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _globalContainer = new GameObject("ProjectilePoolContainer").transform;

            InitPool();
        }

        private void InitPool()
        {
            _pool = new ObjectPool<Projectile>(
                _projectilePrefab,
                _initialPoolSize,
                _globalContainer
            );
        }

        public Projectile Get(Transform towerParent)
        {
            var projectile = _pool.Get();

            projectile.transform.SetParent(towerParent);

            projectile.OnReturnedToPool = () =>
            {
                projectile.transform.SetParent(_globalContainer);
                _pool.ReturnToPool(projectile);
            };

            return projectile;
        }
    }
}