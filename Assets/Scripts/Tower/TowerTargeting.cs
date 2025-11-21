using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;

namespace Tower
{
    public class TowerTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform parentTower;
    
        [Header("Settings")] [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private LayerMask enemyLayer;
        private readonly List<EnemyController> _enemiesInRange = new();
        [SerializeField]
        private EnemyController currentTarget;
        [SerializeField] private CircleCollider2D _collider;
        
        private int _myInstanceID;

        private void Awake()
        {
            _collider.isTrigger = true;
            _collider.radius = detectionRadius;
            
            _myInstanceID = parentTower.GetInstanceID();
        }

        private void Update()
        {
            _enemiesInRange.RemoveAll(e => e == null);

            if (currentTarget == null || !_enemiesInRange.Contains(currentTarget))
            {
                SelectClosestTarget();
            }
        }
        
        public void SetDetectionRadius(float radius)
        {
            detectionRadius = radius;
            
            if(_collider != null) 
                _collider.radius = radius;
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                var enemy = other.GetComponent<EnemyController>();
                if (enemy != null) _enemiesInRange.Add(enemy);
                SelectClosestTarget();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                _enemiesInRange.Remove(enemy);

                if (currentTarget == enemy)
                {
                    currentTarget = null;
                    EventBus.Publish(new TargetLostEvent(_myInstanceID));
                    SelectClosestTarget();
                }
            }
        }

        private void SelectClosestTarget()
        {
            if (_enemiesInRange.Count == 0)
            {
                currentTarget = null;
                return;
            }
            EnemyController closest = null;
            float minDist = float.MaxValue;
            Vector3 towerPos = transform.position;

            foreach (var enemy in _enemiesInRange)
            {
                if (enemy == null) continue;
                float distSq = (enemy.transform.position - towerPos).sqrMagnitude;
                if (distSq < minDist)
                {
                    minDist = distSq;
                    closest = enemy;
                }
            }
            if (closest != null && closest != currentTarget)
            {
                currentTarget = closest;
                GetCurrentTarget();

                EventBus.Publish(new GetTargetEvent(_myInstanceID, closest.transform));
            }
        }

        public Transform GetCurrentTarget() =>
            currentTarget != null ? currentTarget.transform : null;
    }
}