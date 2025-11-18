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
        
        private void Awake()
        {
            _enemyPool = new ObjectPool<EnemyController>(enemyPrefab, prewarmCount, enemyContainer);
         
        }
        private void Start()
        {
            EventBus.Subscribe<PathConstructedEvent>(OnPathConstructed);
        }

        
        private void OnPathConstructed(PathConstructedEvent e)
        {
            Debug.Log($"PathConstructedEvent received! Waypoints: {e.Waypoints.Count}");
            _waypoints = e.Waypoints;
            InvokeRepeating(nameof(SpawnEnemy), 1f, 2f);
        }

        private void SpawnEnemy()
        {
            gameObject.SetActive(true);
            Debug.Log("EnemySpawn");
            var enemy = _enemyPool.GetFromPull(_waypoints[0], Quaternion.identity);
            enemy.Init(_waypoints, () => _enemyPool.ReturnToPool(enemy));
        }
    }
}