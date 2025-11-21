using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
         [SerializeField] private EnemyController enemyPrefab;
         [SerializeField] private Transform enemyContainer;
         [SerializeField] private int prewarmCount = 10;
        

        private ObjectPool<EnemyController> _enemyPool;
        [SerializeField]
        private List<Vector3> waypoints;
        [SerializeField]
        private bool isPathReady = false;
        
        private void Awake() 
        {
            if (enemyPrefab == null)
            {
                enabled = false;
                return;
            }

            if (enemyContainer == null)
            {
                var container = new GameObject("Enemy Container");
                enemyContainer = container.transform;
            }

            _enemyPool = new ObjectPool<EnemyController>(
                enemyPrefab, 
                prewarmCount, 
                enemyContainer
            );
            
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
            
            waypoints = new List<Vector3>(e.Waypoints); 
            isPathReady = true;
            
            InvokeRepeating(nameof(SpawnEnemy), 1f, 2f);
        }

        private void SpawnEnemy()
        {
            if (!isPathReady || waypoints == null || waypoints.Count == 0)
            {
                return;
            }
            
            var enemy = _enemyPool.Get(waypoints[0], Quaternion.identity);
            
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.Init(waypoints, () => _enemyPool.ReturnToPool(enemy));
            }
        }
    }
}
