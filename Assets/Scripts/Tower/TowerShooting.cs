using Core;
using Core.Interfaces;
using Data;
using UnityEngine;

namespace Tower
{
    public class TowerShooting : MonoBehaviour
    {
        private IProjectileFactory _projectileFactory;
        [SerializeField]
        private Transform shootPoint;
        [SerializeField]
        private float fireRate;
        [SerializeField]
        private Transform _currentTarget;
        
        [Header("Debug Info - Runtime Only")]
         [SerializeField]
        private float lastShootTime;

        private float _damage;
        private int _myInstanceID;

        private TowerData _towerData;
        
        private void Awake()
        {
            _projectileFactory = ProjectilePoolFactory.Instance;
            _myInstanceID = transform.GetInstanceID();
            
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GetTargetEvent>(OnTargetLocked);
            EventBus.Subscribe<TargetLostEvent>(OnTargetLost);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GetTargetEvent>(OnTargetLocked);
            EventBus.Unsubscribe<TargetLostEvent>(OnTargetLost);
        }

        private void Update()
        {
            if (_currentTarget == null) return;

            if (Time.time - lastShootTime >= 1f / fireRate)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }

        public void Shoot()
        {
            var projectile = _projectileFactory.Get(transform);
            projectile.transform.position = shootPoint.position;

            Vector2 dir = (_currentTarget.position - shootPoint.position).normalized;
            projectile.Launch(dir, 10f);
        }

        public void SetState(float damage, float newFireRate)
        {
            _damage = damage;
            fireRate = newFireRate;
        }
        

        private void OnTargetLocked(GetTargetEvent e)
        {
            if (e.TowerID != _myInstanceID)
            {
                return; 
            }
            
            _currentTarget = e.Target;
            
            
        }
        
        private void OnTargetLost(TargetLostEvent e)
        {
            if (e.TowerID != _myInstanceID)
            {
                return;
            }
            
            
            _currentTarget = null;
        }
    }
}