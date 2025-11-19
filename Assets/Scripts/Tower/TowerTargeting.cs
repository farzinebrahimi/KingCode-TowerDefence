using System;
using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tower
{
    public class TowerTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _parentTower;
    
        [Header("Settings")] [SerializeField] private float _detectionRadius = 3f;
        [SerializeField] private LayerMask _enemyLayer;
        private readonly List<EnemyController> _enemiesInRange = new();
        [SerializeField]
        private EnemyController _currentTarget;
        [SerializeField] private CircleCollider2D _collider;
        
        private int _myInstanceID;

        private void Awake()
        {
            _collider.isTrigger = true;
            _collider.radius = _detectionRadius;
            
            _myInstanceID = _parentTower.GetInstanceID();
        }

        private void Update()
        {
            _enemiesInRange.RemoveAll(e => e == null);

            if (_currentTarget == null || !_enemiesInRange.Contains(_currentTarget))
            {
                SelectClosestTarget();
            }
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((_enemyLayer.value & (1 << other.gameObject.layer)) != 0)
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

                if (_currentTarget == enemy)
                {
                    _currentTarget = null;
                    EventBus.Publish(new TargetLostEvent(_myInstanceID));
                    SelectClosestTarget();
                }
            }
        }

        private void SelectClosestTarget()
        {
            if (_enemiesInRange.Count == 0)
            {
                _currentTarget = null;
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
            if (closest != null && closest != _currentTarget)
            {
                _currentTarget = closest;
                GetCurrentTarget();

                EventBus.Publish(new GetTargetEvent(_myInstanceID, closest.transform));
            }
        }

        public Transform GetCurrentTarget() =>
            _currentTarget != null ? _currentTarget.transform : null;
    }
}