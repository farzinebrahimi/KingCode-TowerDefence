using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private int prewarmCount = 10;
        

        [SerializeField]
        private ObjectPool<EnemyController> _enemyPool;
        [SerializeField]
        private List<Vector3> _waypoints;
        [SerializeField]
        private bool _isPathReady = false;
        
        private void Awake()
        {
            _enemyPool = new ObjectPool<EnemyController>(enemyPrefab, prewarmCount, enemyContainer);
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe<PathConstructedEvent>(OnPathConstructed);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PathConstructedEvent>(OnPathConstructed);
            CancelInvoke(nameof(SpawnEnemy));
        }
        
        private void OnPathConstructed(PathConstructedEvent e)
        {
            
            if (e.Waypoints == null || e.Waypoints.Count == 0)
            {
                Debug.LogError("[EnemyManager] Waypoints list is empty or null!");
                return;
            }
            
            _waypoints = new List<Vector3>(e.Waypoints); 
            _isPathReady = true;
            
            InvokeRepeating(nameof(SpawnEnemy), 1f, 2f);
        }

        private void SpawnEnemy()
        {
            if (!_isPathReady || _waypoints == null || _waypoints.Count == 0)
            {
                return;
            }
            
            var enemy = _enemyPool.GetFromPull(_waypoints[0], Quaternion.identity);
            
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.Init(_waypoints, () => _enemyPool.ReturnToPool(enemy));
            }
        }
    }
}
