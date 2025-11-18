using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _waypoints;
        [SerializeField]
        private int _currentIndex;
        
        private Action _onDeathReturn;

        public float speed = 2f;

        public void Init(List<Vector3> waypoints, System.Action returnAction)
        {
            _waypoints = waypoints;
            _currentIndex = 0;
            _onDeathReturn = returnAction;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_waypoints == null || _waypoints.Count == 0) return;

            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentIndex], speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _waypoints[_currentIndex]) < 0.1f)
            {
                _currentIndex++;

                if (_currentIndex >= _waypoints.Count)
                {
                    _onDeathReturn?.Invoke(); 
                }
            }
        }
    }
}